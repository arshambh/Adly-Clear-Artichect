using Adly.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Adly.Application.Contracts.User;

public interface IUserManager
{
    Task<IdentityResult> PasswordCreateAsync(UserEntity user,CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task<IdentityResult> ValidatePasswordAsync(UserEntity user,string givenPassword,CancellationToken cancellationToken);
}