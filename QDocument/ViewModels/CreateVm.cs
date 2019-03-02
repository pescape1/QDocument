using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.ViewModels
{
    public class CreateVm
    {
        [Required(ErrorMessage = "Username is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Select a job")]
        [Display(Name = "Job position")]
        public int JobID { get; set; }
    }
}
