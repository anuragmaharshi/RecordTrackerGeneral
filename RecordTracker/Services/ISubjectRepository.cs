using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetSubectsAsync();
        Task<Subject> GetSubectAsync(long id);
        Task<Subject> AddSubectAsync(Subject subject);
        Task<Subject> UpdateSubjectAsync(Subject subject);
        Task DeleteSubectAsync(long Id);
    }
}
