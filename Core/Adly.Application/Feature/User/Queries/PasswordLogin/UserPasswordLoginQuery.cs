using Adly.Application.Common;
using Adly.Application.Contracts.User.Models;
using Mediator;

namespace Adly.Application.Feature.User.Queries.PasswordLogin;

public record UserPasswordLoginQuery(string UsernameOrEmail, string Password):IRequest<OperationResult<JwtAccessTokenModel>>;