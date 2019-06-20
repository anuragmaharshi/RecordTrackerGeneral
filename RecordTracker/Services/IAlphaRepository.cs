using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface IAlphaRepository
    {
        Task<List<Alpha>> GetAlphasAsync();
        Task<Alpha> GetAlphaAsync(long id);
        Task<Alpha> AddAlphaAsync(Alpha alpha);
        Task<Alpha> UpdateAlphaAsync(Alpha alpha);
        Task DeleteAlphaAsync(long Id);
    }
}
