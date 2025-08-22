using Adly.Application.Contracts.User;
using Adly.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Adly.Infrastructure.Identity.Services.Implementations;

internal class UserManagerImplementation(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) :IUserManager
{
    public async Task<IdentityResult> PasswordCreateAsync(UserEntity user, string password, CancellationToken cancellationToken)
    {
        return await userManager.CreateAsync(user, password);
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> ValidatePasswordAsync(UserEntity user, string givenPassword, CancellationToken cancellationToken)
    {
        var checkPassword = await signInManager.CheckPasswordSignInAsync(user, givenPassword, true);

        if (checkPassword.Succeeded)
            return IdentityResult.Success;

        return IdentityResult.Failed(new IdentityError() { Code = "InvalidPassword", Description = "Password is not correct" });
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }
}