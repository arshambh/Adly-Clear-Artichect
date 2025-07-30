using Adly.Application.Common;
using Adly.Application.Repositories.Common;
using Mediator;

namespace Adly.Application.Feature.Location.Queries;

public class GetLocationByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetLocationByIdQuery, OperationResult<GetLocationByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetLocationByIdQueryResult>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await unitOfWork.LocationRepository.GetLocationByIdAsync(request.LocationId, cancellationToken);
        if (location is null)
            return OperationResult<GetLocationByIdQueryResult>.NotFoundResult(nameof(GetLocationByIdQuery.LocationId),
                "Location not found");


        return OperationResult<GetLocationByIdQueryResult>.SuccessResult(
            new GetLocationByIdQueryResult(location.Id, location.Name));

    }
}

