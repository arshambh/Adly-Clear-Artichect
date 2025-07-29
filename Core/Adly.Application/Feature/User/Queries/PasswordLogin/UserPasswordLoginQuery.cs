using Adly.Application.Common;
using Adly.Application.Common.Validation;
using Adly.Application.Contracts.User.Models;
using Adly.Application.Feature.User.Commands.Register;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.User.Queries.PasswordLogin;

public record UserPasswordLoginQuery(string UsernameOrEmail, string Password):IRequest<OperationResult<JwtAccessTokenModel>>, IValidatableModel<UserPasswordLoginQuery>
{
    public IValidator<UserPasswordLoginQuery> Validate(ValidationModelBase<UserPasswordLoginQuery> validator)
    {
        validator.RuleFor(x => x.UsernameOrEmail).NotEmpty();
        validator.RuleFor(x => x.Password).NotEmpty();

        return validator;
    }
}