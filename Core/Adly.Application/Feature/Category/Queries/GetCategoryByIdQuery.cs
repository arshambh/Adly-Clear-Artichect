using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Category.Queries;

public record GetCategoryByIdQuery(Guid CategoryId):IRequest<OperationResult<GetCategoryByIdQueryResult>>,
    IValidatableModel<GetCategoryByIdQuery>
{
    public IValidator<GetCategoryByIdQuery> Validate(ValidationModelBase<GetCategoryByIdQuery> validator)
    {
        validator.RuleFor(x => x.CategoryId)
            .NotEmpty();
        return validator;
    }
}