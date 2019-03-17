using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QDocument.Data.Models;

namespace QDocument.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<QDocument.Data.Models.Document> Documents { get; set; }
        public DbSet<QDocument.Data.Models.User> User { get; set; }
        public DbSet<QDocument.Data.Models.Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<Job>().ToTable("Job");

            //modelBuilder.Entity<DocumentApproval>()
            //    .HasKey(c => new { c.DocumentID, c.UserId });
        }
    }
}
