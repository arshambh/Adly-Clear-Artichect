using Adly.Application.Common;
using Adly.Application.Contracts.FileService.Interface;
using Adly.Application.Contracts.FileService.Models;
using Adly.Application.Repositories.Common;
using Adly.Domain.Common.ValueObjects;
using Mediator;

namespace Adly.Application.Feature.Ad.Commands;

public class EditAdCommandHandler(IUnitOfWork unitOfWork, IFileService fileService) : IRequestHandler<EditAdCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(EditAdCommand request, CancellationToken cancellationToken)
    {

        if (request.CategoryId.HasValue && request.CategoryId != Guid.Empty)
        {
            var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.CategoryId.Value, cancellationToken);
            if (category == null)
                return OperationResult<bool>.FailureResult(nameof(EditAdCommand.CategoryId), "Category not found.");
        }

        if (request.LocationId.HasValue && request.LocationId != Guid.Empty)
        {
            var category = await unitOfWork.LocationRepository.GetLocationByIdAsync(request.LocationId.Value, cancellationToken);
            if (category == null)
                return OperationResult<bool>.FailureResult(nameof(EditAdCommand.LocationId), "Location not found.");
        }


        var ad = await unitOfWork.AdRepository.GetAdDetailByIdAsync(request.AdId, cancellationToken);
        if (ad == null)
            return OperationResult<bool>.FailureResult(nameof(EditAdCommand.AdId), "Ad not found.");


        ad.Edit(request.Title, request.Description, request.CategoryId, request.LocationId);

        if (request.RemovedImageNames.Any())
        {
            ad.RemoveImage(request.RemovedImageNames);
            await fileService.RemoveFilesAsync(request.RemovedImageNames.ToList(), cancellationToken);
        }

        if (request.NewImages.Any())
        {

            var savedNewImages = await fileService.SaveFilesAsync(
                request.NewImages
                .Select(x => new SaveFileModel(x.ImageContent, x.ImageType)).ToList(),
                cancellationToken);


            foreach (var image in savedNewImages)
            {
                ad.AddImage(new ImageValueObjects(image.FileName, image.FileType));
            }
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return OperationResult<bool>.SuccessResult(true);

    }
}