using QDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Look for any documents.
            if (context.Documents.Any())
            {
                return;   // DB has been seeded
            }

            var documents = new Document[]
            {
                new Document { Title = "Control de Documentos", CreationDate = DateTime.Parse("2010-09-01"), DocumentType = DocumentType.Procedure }
            };

            foreach (Document s in documents)
            {
                context.Documents.Add(s);
            }
            context.SaveChanges();
        }
    }
}
