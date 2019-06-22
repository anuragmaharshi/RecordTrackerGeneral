using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface IGammaRepository
    {
        Task<List<Gamma>> GetGammasAsync();
        Task<Gamma> GetGammaAsync(long id);
        Task<Gamma> AddGammaAsync(Gamma gamma);
        Task<Gamma> UpdateGammaAsync(Gamma gamma);
        Task DeleteGammaAsync(long Id);
    }
}
