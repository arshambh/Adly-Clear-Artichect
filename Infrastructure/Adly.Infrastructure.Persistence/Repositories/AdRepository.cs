using Adly.Application.Repositories.Ad;
using Adly.Domain.Entities.Ad;
using Adly.Infrastructure.Persistence.Configurations;
using Adly.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Adly.Infrastructure.Persistence.Repositories;

internal class AdRepository(AdlyDbContext db) : BaseRepository<AdEntity>(db), IAdRepository
{
    public async Task CreateAdAsync(AdEntity adEntity, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(adEntity, cancellationToken);
    }

    public async Task<AdEntity?> GetAdByIdForUpdateAsync(Guid adId, CancellationToken cancellationToken = default)
    {
        return await base.Table
            .FirstOrDefaultAsync(c => c.Id.Equals(adId), cancellationToken: cancellationToken);

    }

    public async Task<AdEntity?> GetAdDetailByIdAsync(Guid adId, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking
            .Include(c => c.User)
            .Include(c => c.Category)
            .Include(c => c.Location)
            .FirstOrDefaultAsync(c => c.Id.Equals(adId), cancellationToken: cancellationToken);
    }

    public async Task<List<AdEntity>> GetUserAdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking
            .Where(c => c.UserId.Equals(userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AdEntity>> GetVerifiedAdsAsync(int currentPage, int pageCount, CancellationToken cancellationToken = default)
    {
        if (pageCount <= 0)
            pageCount = 10;
        if (currentPage <= 0)
            currentPage = 1;

        return await base.TableAsNoTracking
            .Where(c => c.CurrentState == AdEntity.AdState.Approved)
            .Skip((currentPage - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync(cancellationToken);
    }
}