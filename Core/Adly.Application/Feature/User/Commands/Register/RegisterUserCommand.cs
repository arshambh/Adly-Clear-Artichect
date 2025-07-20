using Adly.Application.Common;
using Mediator;

namespace Adly.Application.Feature.User.Commands.Register;


public record RegisterUserCommand(string FirstName
    ,string LastName
    ,string Username
    ,string Email
    ,string PhoneNumber
    ,string Password
    ,string RepeatPassword):IRequest<OperationResult<bool>>;