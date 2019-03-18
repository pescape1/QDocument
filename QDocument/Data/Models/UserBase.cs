using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Models
{
    public abstract class UserBase : IdentityUser
    {
        [Required(ErrorMessage = "First Name es requerido.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name es requerido.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Address es requerido.")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Job position es requerido.")]
        public int JobID { get; set; }

        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}
