using Adly.Domain.Entities.Ad;

namespace Adly.Application.Feature.Ad.Queries;

public record GetUserAdsQueryResult(Guid AdId, string? Title, DateTime ModifiedDate, AdEntity.AdState CurrentState);