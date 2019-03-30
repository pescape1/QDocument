using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        [ForeignKey("Id")]
        public virtual User User { get; set; }
        [ForeignKey("Id")]
        public virtual Role Role { get; set; }
    }
}
