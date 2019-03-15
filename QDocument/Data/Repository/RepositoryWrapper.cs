using QDocument.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Repository
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private readonly ApplicationDbContext _context;
        private IDocumentRepository _document;
        private IJobRepository _job;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IDocumentRepository Document
        {
            get
            {
                if (_document == null)
                {
                    _document = new DocumentRepository(_context);
                }

                return _document;
            }
        }

        public IJobRepository Job
        {
            get
            {
                if (_job == null)
                {
                    _job = new JobRepository(_context);
                }

                return _job;
            }
        }
    }
}
