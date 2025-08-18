using Adly.Application.Repositories.Ad;
using Adly.Application.Repositories.Category;
using Adly.Application.Repositories.Common;
using Adly.Application.Repositories.Location;
using Adly.Infrastructure.Persistence.Configurations;

namespace Adly.Infrastructure.Persistence.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AdlyDbContext _db;


    public UnitOfWork(AdlyDbContext db)
    {
        _db = db;

        // برای جلوگیری از استفاده 
        // DI
        // از ریپازیتوری ها این طوری تعریف کردیم که کاربر مجبور بشه
        // فقط و فقط از 
        // IUnitOfWork
        // استفاده کند

        LocationRepository = new LocationRepository(db);
        CategoryRepository = new CategoryRepository(db);
        AdRepository = new AdRepository(db);
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
    }

    public ILocationRepository LocationRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IAdRepository AdRepository { get; }



    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}