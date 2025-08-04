using Adly.Domain.Entities.Ad;

namespace Adly.Application.Repositories.Location;

public interface ILocationRepository
{
    Task CreateAsync(LocationEntity locationEntity, CancellationToken cancellationToken = default);

    Task<LocationEntity?> GetLocationByIdAsync(Guid locationId,CancellationToken cancellationToken = default);

    Task<List<LocationEntity>> GetLocationByNameAsync(string locationName, CancellationToken cancellationToken = default);

    Task<bool> IsLocationExistsAsync(string locationName, CancellationToken cancellationToken = default);

}