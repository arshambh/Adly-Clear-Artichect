using Adly.Application.Repositories.LocationRepository;

namespace Adly.Application.Repositories.Common;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    ILocationRepository LocationRepository { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}