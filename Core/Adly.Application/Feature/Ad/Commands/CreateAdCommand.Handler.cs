using Adly.Application.Common;
using Adly.Application.Contracts.FileService.Interface;
using Adly.Application.Contracts.FileService.Models;
using Adly.Application.Contracts.User;
using Adly.Application.Repositories.Common;
using Adly.Domain.Common.ValueObjects;
using Adly.Domain.Entities.Ad;
using Mediator;

namespace Adly.Application.Feature.Ad.Commands;

public class CreateAdCommandHandler(IUnitOfWork unitOfWork, IFileService fileService, IUserManager userManager) : IRequestHandler<CreateAdCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        var location = await unitOfWork.LocationRepository.GetLocationByIdAsync(request.LocationId, cancellationToken);
        if (location is null)
            return OperationResult<bool>.FailureResult(nameof(CreateAdCommand.LocationId), "Specified Location not found...!");

        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return OperationResult<bool>.FailureResult(nameof(CreateAdCommand.CategoryId), "Specified Category not found...!");

        var user = await userManager.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return OperationResult<bool>.FailureResult(nameof(CreateAdCommand.UserId), "Specified User not found...!");

        AdEntity ad;

        try
        {
            ad = AdEntity.Create(request.Title, request.Description, request.UserId, request.CategoryId,
                request.LocationId);
        }
        catch (Exception e)
        {
            return OperationResult<bool>.DomainFailureResult(e.Message);
        }

        if (request.AdImages.Any())
        {
            var saveImages = await fileService.SaveFilesAsync(
                request.AdImages.Select(x => new SaveFileModel(x.Base64File, x.FileContent)).ToList(), cancellationToken);


            saveImages.ForEach(x => ad.AddImage(new ImageValueObjects(x.FileName, x.FileType)));

        }

        await unitOfWork.CommitAsync(cancellationToken);
        return OperationResult<bool>.SuccessResult(true);


    }
}