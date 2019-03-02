using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QDocument.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Select a job.")]
        public int JobID { get; set; }

        public Job Job { get; set; }
    }
}
