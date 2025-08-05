using Adly.Application.Repositories.Ad;
using Adly.Application.Repositories.Category;
using Adly.Application.Repositories.Location;

namespace Adly.Application.Repositories.Common;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    ILocationRepository LocationRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IAdRepository AdRepository { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}