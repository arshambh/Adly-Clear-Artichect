using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Category.Queries;

public record GetCategoriesByNameQuery(string CategoryName)
    : IRequest<OperationResult<List<GetCategoriesByNameQueryResult>>>, IValidatableModel<GetCategoriesByNameQuery>
{
    public IValidator<GetCategoriesByNameQuery> Validate(ValidationModelBase<GetCategoriesByNameQuery> validator)
    {
        validator.RuleFor(x => x.CategoryName).NotEmpty();
        return validator;

    }
}