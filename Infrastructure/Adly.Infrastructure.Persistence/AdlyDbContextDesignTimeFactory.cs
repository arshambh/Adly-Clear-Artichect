using Adly.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Adly.Infrastructure.Persistence;

public class AdlyDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AdlyDbContext>
{
    public AdlyDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<AdlyDbContext>()
            .UseSqlServer();

        return new AdlyDbContext(optionBuilder.Options);
    }
}