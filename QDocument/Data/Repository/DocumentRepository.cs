using QDocument.Data.Contracts;
using QDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Document> GetDocumentByIdAsync(int ID)
        {
            var document = await FindByConditionAsync(d => d.ID.Equals(ID));
            return document.DefaultIfEmpty(new Document())
                    .FirstOrDefault();
        }

        public async Task CreateDocumentAsync(Document document)
        {
            Create(document);
            await SaveAsync();
        }

        public async Task UpdateDocumentAsync(Document document)
        {
            Update(document);
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
