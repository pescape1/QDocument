using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Models
{
    public abstract class UserBase : IdentityUser<int>
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

        [ForeignKey("CreationUser")]
        public ICollection<Document> CreatedDocuments { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
    //*********************************************************** Other Identity classes ***************************
    public class UserClaim : IdentityUserClaim<int>
    {
        [ForeignKey("Id")]
        public virtual User User { get; set; }
    }

    public class UserLogin : IdentityUserLogin<int>
    {
        [ForeignKey("Id")]
        public virtual User User { get; set; }
    }

    public class UserToken : IdentityUserToken<int>
    {
        [ForeignKey("Id")]
        public virtual User User { get; set; }
    }
}
