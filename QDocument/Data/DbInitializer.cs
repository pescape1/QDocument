using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QDocument.Data.Models;
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
            if (context.Jobs.Any())
            {
                return;   // DB has been seeded
            }

            var jobs = new Job[]
            {
                new Job { Title = "Quality Manager", ShortTitle = "QA Mgr."}
            };
            foreach (Job j in jobs)
            {
                context.Jobs.Add(j);
            }
            context.SaveChanges();
            /*
            var documents = new Document[]
            {
                new Document { Title = "Control de Documentos", CreationDate = DateTime.Parse("2010-09-01"), DocumentType = DocumentType.Procedure }
            };

            foreach (Document s in documents)
            {
                context.Documents.Add(s);
            }
            context.SaveChanges();*/
        }
    }
}
