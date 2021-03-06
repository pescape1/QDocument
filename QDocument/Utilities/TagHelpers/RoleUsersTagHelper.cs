﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QDocument.Data.Models;

namespace QDocument.Utilities.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project 
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;

        public RoleUsersTagHelper(UserManager<User> userMgr, RoleManager<Role> roleMgr)
        {
            userManager = userMgr;
            roleManager = roleMgr;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            Role role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }

            output.Content.SetContent(names.Count == 0
                ? "No Users"
                : string.Join(", ", names));
        }
    }
}
