using Adly.Application.Common.MappingConfigurations;
using Adly.Application.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Adly.Application.Extensions;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection RegisterApplicationValidator(this IServiceCollection services)
    {
        // Gather candidate types safely (skip dynamic/proxy assemblies and guard GetExportedTypes)
        var validationTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !IsSkippableAssembly(a))
            .SelectMany(SafeGetExportedTypes)
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatableModel<>)));

        foreach (var validationType in validationTypes)
        {
            object? requestModel = null;

            try
            {
                // Use the "widest" ctor, pass nulls (your current approach), but guard against ctor exceptions
                var widestCtorParamCount = validationType
                    .GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .FirstOrDefault()?.GetParameters().Length ?? 0;

                requestModel = Activator.CreateInstance(validationType, new object?[widestCtorParamCount]);
            }
            catch
            {
                // Skip types that cannot be instantiated with null args
                continue;
            }

            if (requestModel is null) continue;

            // Find the Validate method on IValidatableModel<T>
            var validateMethod = validationType.GetMethod(nameof(IValidatableModel<object>.Validate),
                               BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (validateMethod is null) continue;

            object? validationModelBase = null;
            try
            {
                validationModelBase = Activator.CreateInstance(
                    typeof(ValidationModelBase<>).MakeGenericType(validationType));
            }
            catch
            {
                continue;
            }

            if (validationModelBase is null) continue;

            object? validator = null;
            try
            {
                validator = validateMethod.Invoke(requestModel, new[] { validationModelBase });
            }
            catch
            {
                continue;
            }

            if (validator is null) continue;

            var validatorInterface = validator.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

            if (validatorInterface is null) continue;

            services.AddTransient(validatorInterface, _ => validator);
        }

        return services;
    }

    private static bool IsSkippableAssembly(Assembly a)
    {
        var name = a.FullName ?? string.Empty;
        return a.IsDynamic
               || name.StartsWith("DynamicProxyGenAssembly2", StringComparison.Ordinal) // Castle proxy
               || name.StartsWith("Anonymously Hosted DynamicMethods", StringComparison.Ordinal);
    }

    private static IEnumerable<Type> SafeGetExportedTypes(Assembly asm)
    {
        try
        {
            return asm.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            // Use the loadable subset
            return ex.Types.Where(t => t != null)!;
        }
        catch
        {
            // Skip problematic assemblies entirely
            return Array.Empty<Type>();
        }
    }
    public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(RegisterApplicationMappers).Assembly);
        return services;
    }
    public static IServiceCollection AddLoggerFactory(this IServiceCollection services)
    {
        services.AddLogging();
        return services;
    }

}