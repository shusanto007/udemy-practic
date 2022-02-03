﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DLL.Model
{
    public class AppRole : IdentityRole<int>
    {
        public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
    }
}