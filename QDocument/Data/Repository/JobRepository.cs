using Microsoft.EntityFrameworkCore;
using QDocument.Data.Contracts;
using QDocument.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QDocument.Data.Repository
{
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            var jobs = await FindAllAsync();
            return jobs.OrderBy(j => j.Title);
        }

        public async Task<Job> GetJobByIdAsync(int ID)
        {
            var job = await FindByConditionAsync(d => d.ID.Equals(ID));
            return job.DefaultIfEmpty(new Job())
                    .FirstOrDefault();
        }

        public async Task CreateJobAsync(Job job)
        {
            Create(job);
            await SaveAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            Update(job);
            await SaveAsync();
        }

        public async Task DeleteJobAsync(Job job)
        {
            Delete(job);
            await SaveAsync();
        }

        public bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.ID == id);
        }
    }
}
