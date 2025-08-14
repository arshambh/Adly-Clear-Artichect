using Adly.Application.Common;
using Adly.Application.Repositories.Common;
using Mediator;

namespace Adly.Application.Feature.Ad.Queries;

public class GetUserAdsQueryHandler(IUnitOfWork unitOfWork):IRequestHandler<GetUserAdsQuery,OperationResult<List<GetUserAdsQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetUserAdsQueryResult>>> Handle(GetUserAdsQuery request, CancellationToken cancellationToken)
    {
        var userAds = await unitOfWork.AdRepository.GetUserAdsAsync(request.UserId, cancellationToken);

        var result = userAds.Select(x =>
            new GetUserAdsQueryResult(x.Id, x.Title, x.ModifiedDate ?? x.CreatedDate, x.CurrentState)).ToList();

        return OperationResult<List<GetUserAdsQueryResult>>.SuccessResult(result);
    }
}