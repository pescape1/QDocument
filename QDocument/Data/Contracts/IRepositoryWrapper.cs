using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Contracts
{
    public interface IRepositoryWrapper
    {
        IDocumentRepository Document { get; }
        IJobRepository Job { get; }
    }
}
