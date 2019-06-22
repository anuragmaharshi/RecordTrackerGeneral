using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface IThetaRepository
    {
        Task<List<Theta>> GetThetasAsync();
        Task<Theta> GetThetaAsync(long id);
        Task<Theta> AddThetaAsync(Theta subject);
        Task<Theta> UpdateThetaAsync(Theta subject);
        Task DeleteThetaAsync(long Id);
    }
}
