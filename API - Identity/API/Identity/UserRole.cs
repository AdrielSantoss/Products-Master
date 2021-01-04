using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public UserIdentity User { get; set; }
        public Role Role { get; set; }
    }
}
