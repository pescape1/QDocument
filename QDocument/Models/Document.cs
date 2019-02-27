using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Models
{
    public enum DocumentType
    {
        Manual = 1,
        Process = 2,
        Procedure = 3,
        Instruction = 4,
        Format = 5,
        Other = 10
    }
    public class Document
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between 3 and 50 characters lenght.")]
        public string Title { get; set; }
        [Display(Name = "Document Type")]
        [EnumDataType(typeof(DocumentType), ErrorMessage = "Valid Document Type value is required.")]
        public DocumentType DocumentType { get; set; }
        [Required(ErrorMessage = "Creation Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
    }
}
