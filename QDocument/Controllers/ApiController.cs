using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QDocument.Models;

namespace QDocument.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private UserManager<User> userManager;

        public ApiController(UserManager<User> userMgr)
        {
            userManager = userMgr;
        }

        [HttpGet]
        [Route("api/GetApprovalUsers")]
        public IActionResult GetApprovalUsers()
        {
            var usersQuery = from u in userManager.Users
                             orderby u.Email
                            select u;
            var userList = new SelectList(usersQuery.AsNoTracking(), "Id", "Email");
            return this.Ok(userList);
        }
    }
}
