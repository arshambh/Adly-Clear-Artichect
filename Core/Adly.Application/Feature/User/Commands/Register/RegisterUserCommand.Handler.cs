using Adly.Application.Common;
using Adly.Application.Contracts.User;
using Adly.Application.Extensions;
using Adly.Domain.Entities.User;
using Mediator;

namespace Adly.Application.Feature.User.Commands.Register;



public class RegisterUserCommandHandler(IUserManager userManager) : IRequestHandler<RegisterUserCommand, OperationResult<bool>>
{

    public async ValueTask<OperationResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new RegisterUserCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return OperationResult<bool>.FailureResult(validationResult.Errors.ConvertToKeyValuePair());

        var user = new UserEntity(request.FirstName, request.LastName, request.Username, request.Email)
        {
            PhoneNumber = request.PhoneNumber
        };
        var createResult = await userManager.PasswordCreateAsync(user, request.Password, cancellationToken);
        if (createResult.Succeeded)
            return OperationResult<bool>.SuccessResult(true);


        return OperationResult<bool>.FailureResult(createResult.Errors.ConvertToKetKeyValuePair());
    }
}