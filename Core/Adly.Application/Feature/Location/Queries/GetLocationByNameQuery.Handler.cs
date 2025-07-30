using Adly.Application.Common;
using Adly.Application.Repositories.Common;
using Mediator;

namespace Adly.Application.Feature.Location.Queries;

public class GetLocationByNameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetLocationByNameQuery, OperationResult<List<GetLocationByNameQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetLocationByNameQueryResult>>> Handle(GetLocationByNameQuery request, CancellationToken cancellationToken)
    {
        var locations =
            await unitOfWork.LocationRepository.GetLocationByNameAsync(request.LocationNameSearchTerm,
                cancellationToken);

        if (!locations.Any())
            return OperationResult<List<GetLocationByNameQueryResult>>.SuccessResult(Enumerable.Empty<GetLocationByNameQueryResult>().ToList());


        return OperationResult<List<GetLocationByNameQueryResult>>.SuccessResult(locations.Select(x => new GetLocationByNameQueryResult(x.Id, x.Name)).ToList());
    }
}