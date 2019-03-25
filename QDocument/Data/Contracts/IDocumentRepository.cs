using QDocument.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QDocument.Data.Contracts
{
    public interface IDocumentRepository : IRepositoryBase<Document>
    {
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<Document> GetDocumentAsync(Expression<Func<Document, bool>> expression, params Expression<Func<Document, object>>[] includes);
        Task<Document> GetDocumentByIdAsync(int id);
        Task<Document> GetDocumentWithApprovalAsync(int id);
        Task CreateDocumentAsync(Document document, params int[] jobList);
        Task UpdateDocumentAsync(Document document, params int[] jobList);
        Task DeleteDocumentAsync(Document document);
        bool DocumentExists(int id);
    }
}
