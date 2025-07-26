using Adly.Application.Common;
using Adly.Application.Contracts.User;
using Adly.Application.Contracts.User.Models;
using Adly.Application.Extensions;
using Mediator;

namespace Adly.Application.Feature.User.Queries.PasswordLogin;

public class UserPasswordLoginQueryHandler(IUserManager userManager,IJwtService jwtService) : IRequestHandler<UserPasswordLoginQuery, OperationResult<JwtAccessTokenModel>>
{
    public async ValueTask<OperationResult<JwtAccessTokenModel>> Handle(UserPasswordLoginQuery request, CancellationToken cancellationToken)
    {
        // var validator = new UserPasswordLoginQueryValidator();
        // var validationResult = await validator.ValidateAsync(request, cancellationToken);
        // if (!validationResult.IsValid)
        //     return OperationResult<JwtAccessTokenModel>.FailureResult(validationResult.Errors.ConvertToKeyValuePair());



        var user = request.UsernameOrEmail.IsEmail() ?
            await userManager.GetUserByEmailAsync(request.UsernameOrEmail, cancellationToken) :
            await userManager.GetUserByUsernameAsync(request.UsernameOrEmail, cancellationToken);

        if (user is null)
        {
            return OperationResult<JwtAccessTokenModel>.NotFoundResult(nameof(UserPasswordLoginQuery.UsernameOrEmail),"User Not Found...!");
        }

        var loginResult = await userManager.ValidatePasswordAsync(user, request.Password, cancellationToken);
        if (loginResult.Succeeded)
        {
           var accessToken=await jwtService.GenerateTokenAsync(user, cancellationToken);

            return OperationResult<JwtAccessTokenModel>.SuccessResult(accessToken);
        }
        return OperationResult<JwtAccessTokenModel>.FailureResult(nameof(UserPasswordLoginQuery.Password),"Incorrect Password");
    }
}