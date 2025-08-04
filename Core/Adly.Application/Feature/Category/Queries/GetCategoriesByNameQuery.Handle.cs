using Adly.Application.Common;
using Adly.Application.Repositories.Common;
using Mediator;

namespace Adly.Application.Feature.Category.Queries;

public class GetCategoriesByNameQueryHandle(IUnitOfWork unitOfWork):IRequestHandler<GetCategoriesByNameQuery, OperationResult<List<GetCategoriesByNameQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetCategoriesByNameQueryResult>>> Handle(GetCategoriesByNameQuery request, CancellationToken cancellationToken)
    {
     var categories=await unitOfWork.CategoryRepository.GetCategoriesBasedOnNameAsync(request.CategoryName, cancellationToken);


     return OperationResult<List<GetCategoriesByNameQueryResult>>
         .SuccessResult(categories
         .Select(x => new GetCategoriesByNameQueryResult(x.Id, x.Name)).ToList());

    }
}