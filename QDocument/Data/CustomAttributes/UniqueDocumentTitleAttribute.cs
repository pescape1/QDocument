using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using QDocument.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.CustomAttributes
{
    public class UniqueDocumentTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var property = validationContext.ObjectType.GetProperty("ID");
            int ID = (int) property.GetValue(validationContext.ObjectInstance, null);
            if (value != null)
            {
                if (value is string title)
                {
                    Document document = _context.Documents.SingleOrDefault(d => d.Title == title && d.ID != ID);
                    if (document != null)
                    {
                        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
