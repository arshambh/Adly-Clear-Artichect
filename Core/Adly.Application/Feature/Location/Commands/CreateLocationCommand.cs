using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Location.Commands;

public record CreateLocationCommand(string LocationName) 
    : IRequest<OperationResult<bool>>, IValidatableModel<CreateLocationCommand>
{
    public IValidator<CreateLocationCommand> Validate(ValidationModelBase<CreateLocationCommand> validator)
    {
        validator.RuleFor(x => x.LocationName)
            .NotEmpty()
            .WithMessage("Location name is required.");


        return validator;
    }
}