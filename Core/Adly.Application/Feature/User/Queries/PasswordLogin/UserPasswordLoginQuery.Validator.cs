using FluentValidation;

namespace Adly.Application.Feature.User.Queries.PasswordLogin;

public class UserPasswordLoginQueryValidator:AbstractValidator<UserPasswordLoginQuery>
{
    public UserPasswordLoginQueryValidator()
    {
        RuleFor(x => x.UsernameOrEmail).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}