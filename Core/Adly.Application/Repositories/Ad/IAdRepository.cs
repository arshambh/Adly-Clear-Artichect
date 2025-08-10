using Adly.Domain.Entities.Ad;

namespace Adly.Application.Repositories.Ad;

public interface IAdRepository
{
    Task CreateAdAsync(AdEntity adEntity, CancellationToken cancellationToken = default);

    Task<AdEntity?> GetAdByIdAsync(Guid adId, CancellationToken cancellationToken = default);

    Task<AdEntity[]> GetUserAdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<AdEntity[]> GetVerifiedAdsAsync(int currentPAge, int pageCount, CancellationToken cancellationToken = default);




}