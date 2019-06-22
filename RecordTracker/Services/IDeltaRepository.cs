using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface IDeltaRepository
    {
        Task<List<Delta>> GetDeltasAsync();
        Task<Delta> GetDeltaAsync(long id);
        Task<Delta> AddDeltaAsync(Delta delta);
        Task<Delta> UpdateDeltaAsync(Delta delta);
        Task DeleteDeltaAsync(long Id);
    }
}
