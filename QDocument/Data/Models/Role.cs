using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Models
{
    public class Role : IdentityRole<int>
    {
        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }

    //******************************** Other identity clasess **********************************
    public class RoleClaim : IdentityRoleClaim<int>
    {
        [ForeignKey("Id")]
        public virtual Role Role { get; set; }
    }

}
