using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QDocument.Data.Models;

namespace QDocument.Data.ViewModels
{
    public class EditRoleVm
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }
}
