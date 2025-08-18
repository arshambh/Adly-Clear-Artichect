using Adly.Application.Repositories.Location;
using Adly.Domain.Entities.Ad;
using Adly.Infrastructure.Persistence.Configurations;
using Adly.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Adly.Infrastructure.Persistence.Repositories;

internal class LocationRepository(AdlyDbContext db):BaseRepository<LocationEntity>(db),ILocationRepository
{
    public async Task CreateAsync(LocationEntity locationEntity, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(locationEntity, cancellationToken);
    }

    public async Task<LocationEntity?> GetLocationByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking.FirstOrDefaultAsync(x => x.Id.Equals(locationId), cancellationToken);
    }

    public async Task<List<LocationEntity>> GetLocationByNameAsync(string locationName, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking.Where(x => x.Name.Contains(locationName)).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsLocationExistsAsync(string locationName, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking.AnyAsync(x => x.Name.Equals(locationName), cancellationToken);
    }
}