﻿using Adly.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Adly.Domain.Entities.User;

public class UserTokenEntity:IdentityUserToken<Guid>,IEntity
{
    public DateTime CreateDate { get; set; }
    public DateTime? ModifiedDate { get; set; }


    public UserEntity User { get; set; }
}