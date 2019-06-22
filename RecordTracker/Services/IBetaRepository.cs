using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RecordTracker.Services
{
    public interface IBetaRepository
    {
        Task<List<Beta>> GetBetasAsync();
        Task<Beta> GetBetaAsync(long id);
        Task<Beta> AddBetaAsync(Beta beta);
        Task<Beta> UpdateBetaAsync(Beta beta);
        Task DeleteBetaAsync(long Id);
    }
}
