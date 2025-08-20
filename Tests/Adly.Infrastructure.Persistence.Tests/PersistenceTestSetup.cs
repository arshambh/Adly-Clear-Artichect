using Adly.Application.Extensions;
using Adly.Infrastructure.Persistence.Configurations;
// namespace Adly.Infrastructure.Persistence.Tests;
//
//
// public class PersistenceTestSetup : IAsyncLifetime
// {
//     private readonly MsSqlContainer _msSqlContainer;
//
//     /// <summary>
//     /// گزینه‌های لازم برای ساخت یک نمونه از AdlyDbContext را در اختیار تست‌ها قرار می‌دهد.
//     /// این کار تضمین می‌کند که هر تست می‌تواند DbContext ایزوله خود را بسازد.
//     /// </summary>
//     public DbContextOptions<AdlyDbContext> DbContextOptions { get; private set; }
//
//     public PersistenceTestSetup()
//     {
//         // 1. ساخت (Build) کانتینر با یک پسورد مشخص و قوی.
//         // این کار قبل از هر عملیات آسنکرون در سازنده (Constructor) انجام می‌شود.
//         _msSqlContainer = new MsSqlBuilder()
//             // .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // می‌توانید از نسخه‌های دیگر هم استفاده کنید
//             // .WithPassword("yourStrong(!)Password123")
//             .Build();
//     }
//
//     /// <summary>
//     /// این متد توسط xUnit قبل از اجرای اولین تست در کلاس فراخوانی می‌شود.
//     /// در اینجا کانتینر داکر را به صورت آسنکرون استارت می‌کنیم.
//     /// </summary>
//     public async Task InitializeAsync()
//     {
//         // 2. استارت کردن کانتینر. این یک عملیات زمان‌بر است.
//         await _msSqlContainer.StartAsync();
//
//         // 3. ایجاد DbContextOptions با استفاده از Connection String داینامیک از کانتینر در حال اجرا.
//         DbContextOptions = new DbContextOptionsBuilder<AdlyDbContext>()
//             .UseSqlServer(_msSqlContainer.GetConnectionString())
//             .Options;
//
//         // 4. ساخت یک نمونه موقت از DbContext فقط برای اطمینان از اجرای Migration ها.
//         // بلوک 'await using' تضمین می‌کند که این نمونه پس از استفاده فورا Dispose می‌شود.
//         await using (var db = new AdlyDbContext(DbContextOptions))
//         {
//             await db.Database.MigrateAsync();
//         }
//     }
//
//     /// <summary>
//     /// این متد توسط xUnit بعد از اتمام تمام تست‌های کلاس فراخوانی می‌شود.
//     /// در اینجا کانتینر را به طور کامل متوقف و منابع آن را پاک‌سازی می‌کنیم.
//     /// </summary>
//     public async Task DisposeAsync()
//     {
//         // 5. فراخوانی DisposeAsync کانتینر را متوقف کرده و تمام منابع مرتبط با آن را آزاد می‌کند.
//         await _msSqlContainer.DisposeAsync();
//     }
// }
//

using Adly.Infrastructure.Persistence.Configurations;
using Adly.Infrastructure.Persistence.Repositories.Common;
using Adly.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Adly.Infrastructure.Persistence.Extensions;
using Testcontainers.MsSql;
using Testcontainers.MsSql;
using Xunit;

namespace Adly.Infrastructure.Persistence.Tests;

public class PersistenceTestSetup : IAsyncLifetime
{
    public UnitOfWork UnitOfWork { get; set; }

    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public IServiceProvider ServiceProvider { get;private set; }


    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        var dbOptionBuilder = new DbContextOptionsBuilder<AdlyDbContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString());

        var db = new AdlyDbContext(dbOptionBuilder.Options);
        await db.Database.MigrateAsync();
        UnitOfWork = new UnitOfWork(db);


        var configs = new Dictionary<string, string>()
        {
            { "ConnectionStrings:AdlyDb", _msSqlContainer.GetConnectionString() }
        };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(configs!);




        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApplicationAutoMapper()
            .AddApplicationMediatorServices()
            .RegisterApplicationValidator()
            .AddPersistenceDbContext(configurationBuilder.Build());

        ServiceProvider = serviceCollection.BuildServiceProvider(false);


    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}