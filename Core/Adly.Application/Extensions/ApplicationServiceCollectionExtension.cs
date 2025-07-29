using Adly.Application.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Adly.Application.Extensions;

public static class ApplicationServiceCollectionExtension
{

    public static IServiceCollection RegisterApplicationValidator(this IServiceCollection services)
    {
        // Register all validators in the assembly

        var validationTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetExportedTypes())
            .Where(x => x.GetInterfaces().Any(m =>
                m.IsGenericType && m.GetGenericTypeDefinition() == typeof(IValidatableModel<>)));

        foreach (var validationType in validationTypes)
        {
            var biggestConstractorLength = validationType.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length).First().GetParameters().Length;

            var requestModel = Activator.CreateInstance(validationType, new object?[biggestConstractorLength]);

            if (requestModel is null)
                continue;

            var requestMethodInfo = validationType.GetMethod(nameof(IValidatableModel<object>.Validate));
            var validationModelBase =
                Activator.CreateInstance(typeof(ValidationModelBase<>).MakeGenericType(validationType));


            if (validationModelBase is null)
                continue;


            var validator = requestMethodInfo?.Invoke(requestModel, [validationModelBase]);

            if (validator is null)
                continue;

            var validatorInterface = validator.GetType()
                .GetInterfaces().FirstOrDefault(m =>
                    m.IsGenericType && m.GetGenericTypeDefinition() == typeof(IValidator<>));

            if (validatorInterface is null)
                continue;

            services.AddTransient(validatorInterface, _ => validator);
        }


        return services;
    }


}