using Adly.Application.Common;
using Adly.Application.Common.Validation;
using Adly.Application.Contracts.FileService.Interface;
using Adly.Application.Repositories.Common;
using Adly.Domain.Entities.Ad;
using AutoMapper;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Ad.Queries;

public class GetAdDetailByIdQueryHandler(IUnitOfWork unitOfWork,IFileService fileService,IMapper mapper):IRequestHandler<GetAdDetailByIdQuery, OperationResult<GetAdDetailByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetAdDetailByIdQueryResult>> Handle(GetAdDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var ad = await unitOfWork.AdRepository.GetAdDetailByIdAsync(request.AdId, cancellationToken);
        if (ad is null)
            return OperationResult<GetAdDetailByIdQueryResult>.FailureResult(nameof(GetAdDetailByIdQuery.AdId),
                "Specified Ad not found");


        var adImages =
            await fileService.GetFilesByNameAsync(ad.Images.Select(x => x.FileName).ToList(), cancellationToken);

        var result = mapper.Map<AdEntity, GetAdDetailByIdQueryResult>(ad);

        result.AdImages = adImages.Select(x => new GetAdDetailByIdQueryResult.AdDetailImageModel(x.FileName, x.FileUrl))
            .ToArray();

        return OperationResult<GetAdDetailByIdQueryResult>.SuccessResult(result);

    }
}