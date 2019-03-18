using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Models
{
    public class UserInput: UserBase
    {
        [Required(ErrorMessage = "E-mail es requerido.")]
        [EmailAddress(ErrorMessage = "E-mail debe ser una dirección válida.")]
        [Display(Name = "E-mail")]
        public override string Email { get; set; }

        [Required(ErrorMessage = "Password es requerido.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password y confirmation password deben coincidir.")]
        public string ConfirmPassword { get; set; }
    }
}
