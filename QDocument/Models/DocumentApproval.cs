using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Models
{
    public class DocumentApproval
    {
        [Key]
        public int ID { get; set; }
        public int DocumentID { get; set; }

        public Document ApprovedDocuments;
        public User ApprovalUsers;
    }
}
