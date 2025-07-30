using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Location.Queries;

public record GetLocationByIdQuery(Guid LocationId):IRequest<OperationResult<GetLocationByIdQueryResult>>,IValidatableModel<GetLocationByIdQuery>
{
    public IValidator<GetLocationByIdQuery> Validate(ValidationModelBase<GetLocationByIdQuery> validator)
    {

        validator.RuleFor(x => x.LocationId)
            .NotEmpty();

        return validator;
    }
}