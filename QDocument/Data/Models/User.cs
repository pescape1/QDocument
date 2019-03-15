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
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Select a job.")]
        public int JobID { get; set; }

        public Job Job { get; set; }
        public ICollection<DocumentApproval> DocumentApproved { get; set; }

        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}
