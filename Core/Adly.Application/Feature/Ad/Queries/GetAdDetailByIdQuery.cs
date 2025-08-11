using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Ad.Queries;

public record GetAdDetailByIdQuery(Guid AdId):IValidatableModel<GetAdDetailByIdQuery>, IRequest<OperationResult<GetAdDetailByIdQueryResult>>
{
    public IValidator<GetAdDetailByIdQuery> Validate(ValidationModelBase<GetAdDetailByIdQuery> validator)
    {
        validator.RuleFor(x => x.AdId)
            .NotEmpty();

        return validator;
    }
}