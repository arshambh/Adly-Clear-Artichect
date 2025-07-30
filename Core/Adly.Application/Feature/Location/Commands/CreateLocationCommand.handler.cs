using Adly.Application.Common;
using Adly.Application.Repositories.Common;
using Mediator;

namespace Adly.Application.Feature.Location.Commands;

public class CreateLocationCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<CreateLocationCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.LocationRepository.IsLocationExistsAsync(request.LocationName, cancellationToken))
            return OperationResult<bool>.FailureResult(nameof(CreateLocationCommand.LocationName),
                "This location name already exists.");

        var locationEntity = new Domain.Entities.Ad.LocationEntity(request.LocationName);
        await unitOfWork.LocationRepository.CreateAsync(locationEntity, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);


        return OperationResult<bool>.SuccessResult(true);
    }
}