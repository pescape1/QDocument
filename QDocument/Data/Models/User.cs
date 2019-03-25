using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QDocument.Data.Models
{
    public class User : UserBase
    {
        public Job Job { get; set; }
    }
}
