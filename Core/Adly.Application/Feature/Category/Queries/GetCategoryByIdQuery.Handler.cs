using Adly.Application.Common;
using Adly.Application.Repositories.Common;
using Mediator;

namespace Adly.Application.Feature.Category.Queries;

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCategoryByIdQuery, OperationResult<GetCategoryByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetCategoryByIdQueryResult>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return OperationResult<GetCategoryByIdQueryResult>.NotFoundResult(nameof(GetCategoryByIdQuery.CategoryId), "Specified category not found...");


        return OperationResult<GetCategoryByIdQueryResult>.SuccessResult(new GetCategoryByIdQueryResult(category.Id, category.Name));

    }
}