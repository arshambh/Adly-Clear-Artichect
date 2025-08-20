using Adly.Application.Feature.Common;
using Adly.Application.Repositories.Common;
using Adly.Infrastructure.Persistence.Configurations;
using Adly.Infrastructure.Persistence.Repositories.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adly.Infrastructure.Persistence.Extensions;

public static class PersistenceServiceCollectionExtension
{

    public static IServiceCollection AddPersistenceDbContext(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<AdlyDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AdlyDb"), builder =>
            {
                builder.EnableRetryOnFailure();
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

}