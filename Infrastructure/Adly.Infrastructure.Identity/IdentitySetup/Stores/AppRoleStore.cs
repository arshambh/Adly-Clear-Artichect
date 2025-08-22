using Adly.Domain.Entities.User;
using Adly.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Adly.Infrastructure.Identity.IdentitySetup.Stores;

internal class AppRoleStore(AdlyDbContext context, IdentityErrorDescriber? describer = null)
    : RoleStore<RoleEntity, AdlyDbContext, Guid>(context, describer);