using Adly.Application.Extensions;
using Adly.Infrastructure.Identity.Extensions;
using Adly.Infrastructure.Persistence;
using Adly.Infrastructure.Persistence.Configurations;
using Adly.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace Adly.Identity.Tests;

public class IdentityTestSetup : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("your_strong_password_123")
        .Build();

    public IServiceProvider ServiceProvider { get; private set; }

    public async Task InitializeAsync()
    {
        try
        {
            await _sqlContainer.StartAsync();
        }
        catch (Exception ex)
        {
            var logs = await _sqlContainer.GetLogsAsync();
            Console.WriteLine($"Container logs: {logs.Stderr} -- {logs.Stdout}");
            throw;
        }
        var dbOptionBuilder = new DbContextOptionsBuilder<AdlyDbContext>()
            .UseSqlServer(_sqlContainer.GetConnectionString());

        var db = new AdlyDbContext(dbOptionBuilder.Options);

        await db.Database.MigrateAsync();

        var configs = new Dictionary<string, string>()
        {
            { "ConnectionStrings:AdlyDb", _sqlContainer.GetConnectionString() },
            { "JwtConfiguration:SignInKey", "ShouldBe-LongerThan-16Char-SecretKey" },
            { "JwtConfiguration:Audience", "TestAud" },
            { "JwtConfiguration:Issuer", "TestIssuer" },
            { "JwtConfiguration:ExpirationMinute", "60" },
            { "JwtConfiguration:EncryptionKey", "16CharEncryptKey" }
        };

        var configurationBuilder = new ConfigurationBuilder();


        var inMemoryConfigs = new MemoryConfigurationSource() { InitialData = configs };

        configurationBuilder.Add(inMemoryConfigs);

        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddApplicationAutoMapper()
            .AddApplicationMediatorServices()
            .RegisterApplicationValidator()
            .AddPersistenceDbContext(configurationBuilder.Build())
            .AddIdentityServices(configurationBuilder.Build())
            .AddLogging(builder => builder.AddConsole());

        ServiceProvider = serviceCollection.BuildServiceProvider(false);
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.StopAsync();
    }
}