using QDocument.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Contracts
{
    public interface IDocumentRepository : IRepositoryBase<Document>
    {
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<Document> GetDocumentByIdAsync(int ID);
        Task CreateDocumentAsync(Document document);
        Task UpdateDocumentAsync(Document document);
        Task DeleteDocumentAsync(Document document);
        bool DocumentExists(int id);
    }
}
