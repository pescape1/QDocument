using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Models
{
    public class Job
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between 3 and 50 characters lenght.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Short Title is required.")]
        [Display(Name = "Short Title")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} must be between 3 and 15 characters lenght.")]
        public string ShortTitle { get; set; }
    }
}
