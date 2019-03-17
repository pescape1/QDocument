using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QDocument.Data.Models;

namespace QDocument.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ApiDocController : ControllerBase
    {
        private UserManager<User> userManager;

        public ApiDocController(UserManager<User> userMgr)
        {
            userManager = userMgr;
        }

        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("api/GetApprovalUsers")]
        //[HttpGet("{jobsList}")]
        public IActionResult GetApprovalUsers(string jobList)
        {
            List<string> jList = JsonConvert.DeserializeObject<List<string>>(jobList);
            var users = userManager.Users
                .Include(u => u.Job)
                .AsNoTracking()
                .Where(u => jList.Contains(u.JobID.ToString()))
                .Select(u => new
                {
                    id = u.Id,
                    FullName = u.FullName(),
                    JobTitle = u.Job.Title
                }).ToArray();
            return Ok(users);
        }
    }
}
