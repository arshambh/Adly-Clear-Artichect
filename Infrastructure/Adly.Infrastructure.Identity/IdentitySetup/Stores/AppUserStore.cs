using Adly.Domain.Entities.User;
using Adly.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Adly.Infrastructure.Identity.IdentitySetup.Stores;

internal class AppUserStore(AdlyDbContext context, IdentityErrorDescriber? describer = null)
    : UserStore<UserEntity, RoleEntity, AdlyDbContext, Guid, UserClaimEntity, UserRoleEntity, UserLoginEntity,
        UserTokenEntity, RoleClaimEntity>(context, describer);