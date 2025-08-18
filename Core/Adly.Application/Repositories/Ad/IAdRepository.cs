using Adly.Domain.Entities.Ad;

namespace Adly.Application.Repositories.Ad;

public interface IAdRepository
{
    Task CreateAdAsync(AdEntity adEntity, CancellationToken cancellationToken = default);

    Task<AdEntity?> GetAdByIdForUpdateAsync(Guid adId, CancellationToken cancellationToken = default);

    Task<AdEntity?> GetAdDetailByIdAsync(Guid adId, CancellationToken cancellationToken = default);

    Task<List<AdEntity>> GetUserAdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<List<AdEntity>> GetVerifiedAdsAsync(int currentPAge, int pageCount, CancellationToken cancellationToken = default);




}