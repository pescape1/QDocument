using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.ViewModels
{
    public class UserInput
    {
        [Required(ErrorMessage = "First Name es requerido.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name es requerido.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-mail es requerido.")]
        [EmailAddress(ErrorMessage = "E-mail debe ser una dirección válida.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password es requerido.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password y confirmation password deben coincidir.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Job position es requerido.")]
        [Display(Name = "Job position")]
        public int JobID { get; set; }
    }
}
