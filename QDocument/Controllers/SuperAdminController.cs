using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QDocument.Data.Contracts;
using QDocument.Data.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860 

namespace QDocument.Controllers
{
    [Authorize] //(Roles = "SuperAdmins")
    public class SuperAdminController : Controller
    {
        private UserManager<User> userManager;
        private IRepositoryWrapper _repoWrapper;
        private IUserValidator<User> userValidator;
        private IPasswordValidator<User> passwordValidator;
        private IPasswordHasher<User> passwordHasher;

        private readonly User testUser = new User
        {
            UserName = "TestTestForPassword",
            Email = "testForPassword@test.test",
            JobID = 0
        };

        public SuperAdminController(UserManager<User> userMgr,
            IRepositoryWrapper repoWrapper,
            IUserValidator<User> userValid, IPasswordValidator<User> passValid,
            IPasswordHasher<User> passHasher)
        {
            userManager = userMgr;
            _repoWrapper = repoWrapper;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passHasher;
        }

        // GET: /<controller>/ 
        public ViewResult Index()
        {
            return View(userManager.Users);
        }

        public async Task<ViewResult> Create()
        {
            await PopulateJobsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserInput userInput)
        {
            User user = new User
            {
                UserName = userInput.Email,
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                Address = userInput.Address,
                Email = userInput.Email,
                JobID = userInput.JobID,
            };
            if (ModelState.IsValid)
            {
                IdentityResult result = await userManager.CreateAsync(user, userInput.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            await PopulateJobsDropDownList(user.JobID);
            return View(user);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
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
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Index", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                await PopulateJobsDropDownList(user.JobID);
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // the names of its parameters must be the same as the property of the User class if we use asp-for in the view 
        // otherwise form values won't be passed properly 
        public async Task<IActionResult> Edit(string id, string userName, string firstName, string lastName, string address, string email, string password, int JobID)
        {
            Boolean updateInfo = false;
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Validate UserName and Email  
                user.UserName = email; // UserName won't be changed in the database until UpdateAsync is executed successfully 
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Address = address;
                user.Email = email;
                user.JobID = JobID;

                IdentityResult validUseResult = await userValidator.ValidateAsync(userManager, user);
                if (!validUseResult.Succeeded)
                {
                    AddErrors(validUseResult);
                }

                if (password != null)
                {
                    testUser.JobID = JobID;
                    // Validate password 
                    // Step 1: using built in validations 
                    IdentityResult passwordResult = await userManager.CreateAsync(testUser, password);
                    if (passwordResult.Succeeded)
                    {
                        await userManager.DeleteAsync(testUser);
                    }
                    else
                    {
                        AddErrors(passwordResult);
                    }
                    /* Step 2: Because of DI, IPasswordValidator<User> is injected into the custom password validator.  
                       So the built in password validation stop working here */
                    IdentityResult validPasswordResult = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPasswordResult.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrors(validPasswordResult);
                    }
                    updateInfo = passwordResult.Succeeded && validPasswordResult.Succeeded;
                }
                else
                {
                    updateInfo = true;
                }

                // Update user info 
                if (validUseResult.Succeeded && updateInfo)
                {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed  
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                    else
                    {
                        await PopulateJobsDropDownList(user.JobID);
                        AddErrors(result);
                    }
                }
            }
            else
            {
                await PopulateJobsDropDownList(user.JobID);
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }

        private async Task PopulateJobsDropDownList(object selectedJob = null)
        {
            var jobsQuery = await _repoWrapper.Job.GetAllJobsAsync();
            ViewBag.JobID = new SelectList(jobsQuery, "ID", "Title", selectedJob);
        }
    }
}
