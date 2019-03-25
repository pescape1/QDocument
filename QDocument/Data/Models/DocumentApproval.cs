using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Models
{
    public class DocumentApproval
    {
        public int DocumentID { get; set; }
        public int JobID { get; set; }

        public Document ApprovedDocuments;
        public Job ApprovalJobs;
    }
}
