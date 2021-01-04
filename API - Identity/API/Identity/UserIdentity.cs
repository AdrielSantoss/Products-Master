using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Identity
{
    public class UserIdentity : IdentityUser<int>
    {
        [Column(TypeName = "nvarchar(150)")]
        public string Fullname { get; set; }
        public List<UserRole> UserRoles { get;  set; }
    }
}
