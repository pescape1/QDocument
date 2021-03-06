﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QDocument.Data.Models;
using QDocument.Data.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860 

namespace QDocument.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private RoleManager<Role> roleManager;
        private UserManager<User> userManager;

        public RoleController(RoleManager<Role> roleMgr, UserManager<User> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new Role { Name = name });

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View((object) name);
        }

        public async Task<IActionResult> Edit(string id)
        {
            Role role = await roleManager.FindByIdAsync(id);
            List<User> members = new List<User>();
            List<User> nonMember = new List<User>();

            foreach (User user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name)
                    ? members
                    : nonMember;
                list.Add(user);
            }

            return View(new EditRoleVm
            {
                Role = role,
                Members = members,
                NonMembers = nonMember
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModifyRoleVm modifyRole)
        {
            IdentityResult result;

            if (ModelState.IsValid)
            {
                Role role = await roleManager.FindByIdAsync(modifyRole.RoleId);
                role.Name = modifyRole.RoleName;
                result = await roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    AddErrors(result);
                }

                foreach (string userId in modifyRole.IdsToAdd ?? new string[] { })
                {
                    User user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, modifyRole.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                        }
                    }
                }

                foreach (string userId in modifyRole.IdsToRemove ?? new string[] { })
                {
                    User user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, modifyRole.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(modifyRole.RoleId);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Role role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }

            return View("Index", roleManager.Roles);
        }
    }
}