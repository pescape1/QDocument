using Microsoft.EntityFrameworkCore;
using QDocument.Data.Contracts;
using QDocument.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QDocument.Data.Repository
{
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            var documents = await FindAllAsync();
            return documents.OrderBy(d => d.Title);
        }

        public async Task<Document> GetDocumentAsync(Expression<Func<Document, bool>> filter, params Expression<Func<Document, object>>[] includes)
        {
            var document = await FindByConditionAsync(filter, includes);
            return document.DefaultIfEmpty(new Document())
                    .FirstOrDefault();
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _context.Documents.SingleOrDefaultAsync(d => d.ID == id);
        }

        public async Task<Document> GetDocumentWithApprovalAsync(int id)
        {
            return await _context.Documents.Include(a => a.ApprovedBy).SingleOrDefaultAsync(d => d.ID == id);
        }

        public async Task CreateDocumentAsync(Document document, params int[] jobList)
        {
            document.ApprovedBy = new List<DocumentApproval>();
            foreach (var job in jobList)
            {
                document.ApprovedBy.Add(new DocumentApproval { DocumentID = document.ID, JobID = job });
            }
            Create(document);
            await SaveAsync();
        }

        public async Task UpdateDocumentAsync(Document document, params int[] jobList)
        {
            var docToUpdate = await GetDocumentWithApprovalAsync(document.ID);

            docToUpdate.Title = document.Title;
            docToUpdate.DocumentType = document.DocumentType;
            docToUpdate.CreationDate = document.CreationDate;

            int[] docAppList = docToUpdate.ApprovedBy.Select(a => a.JobID).ToArray();

            int[] docAppJob = docAppList.Union(jobList).ToArray();

            foreach (var job in docAppJob)
            {
                if (jobList == null || !jobList.Contains(job))
                {
                    DocumentApproval docToRemove = docToUpdate.ApprovedBy.SingleOrDefault(a => a.DocumentID == document.ID && a.JobID == job);
                    docToUpdate.ApprovedBy.Remove(docToRemove);
                }
                if (docAppList == null || !docAppList.Contains(job))
                {
                    docToUpdate.ApprovedBy.Add(new DocumentApproval { JobID = job });
                }
            }

            Update(docToUpdate);

            await SaveAsync();
        }

        public async Task DeleteDocumentAsync(Document document)
        {
            Delete(document);
            await SaveAsync();
        }

        public bool DocumentExists(int id)
        {
            return _context.Jobs.Any(e => e.ID == id);
        }
    }
}
