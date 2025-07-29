using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.User.Commands.Register;


public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string PhoneNumber,
    string Password,
    string RepeatPassword) : IRequest<OperationResult<bool>>, IValidatableModel<RegisterUserCommand>
{
   

    public IValidator<RegisterUserCommand> Validate(ValidationModelBase<RegisterUserCommand> validator)
    {
        validator.RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");

        validator.RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        validator.RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.");

        validator.RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        validator.RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.");

        validator.RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

        validator.RuleFor(x => x.RepeatPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");

        return validator;
    }
}