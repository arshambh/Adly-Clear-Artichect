using Adly.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Adly.Domain.Entities.User;

public class RoleClaimEntity:IdentityRoleClaim<Guid>,IEntity
{
    public DateTime CreateDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public RoleEntity Role { get; set; }

}