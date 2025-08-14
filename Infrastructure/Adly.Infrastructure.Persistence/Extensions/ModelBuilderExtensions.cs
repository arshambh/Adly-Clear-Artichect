using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Adly.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void RegisterEntities<TEntityType>(this ModelBuilder builder, params Assembly[] assemblies)
    {
        var entityTypes = assemblies.SelectMany(x => x.ExportedTypes)
            .Where(x => x is { IsClass: true, IsAbstract: false, IsPublic: true } &&
                        typeof(TEntityType).IsAssignableFrom(x) && !x.IsGenericTypeDefinition);

        foreach (var entityType in entityTypes)
        {
            try
            {
                builder.Entity(entityType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to register entity '{entityType.FullName}'. " +
                    $"Check for empty names in [Table]/[View]/[Column]/[Index] or ToTable/ToView/etc.", ex);
            }
        }

    }

    public static void ApplyRestrictDeleteBehaviour(this ModelBuilder modelBuilder)
    {
        var cascadeForeignKeys = modelBuilder.Model.GetEntityTypes()
            .SelectMany(x => x.GetForeignKeys())
            .Where(x => x is { IsOwnership: false, DeleteBehavior: DeleteBehavior.Cascade });

        foreach (var cascadeForeignKey in cascadeForeignKeys)
        {
            cascadeForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }


    }



}