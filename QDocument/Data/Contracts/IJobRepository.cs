using Microsoft.EntityFrameworkCore;
using QDocument.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Contracts
{
    public interface IJobRepository : IRepositoryBase<Job>
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<Job> GetJobByIdAsync(int ID);
        Task CreateJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(Job job);
        bool JobExists(int id);
    }
}