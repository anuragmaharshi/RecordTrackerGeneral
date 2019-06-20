using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface ISourceRepository
    {
        Task<List<Source>> GetSourcesAsync();
        Task<Source> GetSourceAsync(long id);
        Task<Source> AddSourceAsync(Source source);
        Task<Source> UpdateSourceAsync(Source source);
        Task DeleteSourceAsync(long Id);
    }
}
