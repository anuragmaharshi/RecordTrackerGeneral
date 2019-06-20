using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RecordTracker.SqliteDataLayer;

namespace RecordTracker.Services
{
    public class SourceRepository : ISourceRepository
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();
  
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());

        public async Task<Source> AddSourceAsync(Source source)
        {
            _context.Sources.Add(source);
            await _context.SaveChangesAsync();
            return source;
        }

        public async Task DeleteSourceAsync(long Id)
        {
            var source = _context.Sources.FirstOrDefault(c => c.Id == Id);
            if (source != null)
            {
                _context.Sources.Remove(source);

            }
            await _context.SaveChangesAsync();
        }

        public async Task<Source> GetSourceAsync(long id)
        {
            return await _context.Sources.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<List<Source>> GetSourcesAsync()
        {
            try
            {
                return _context.Sources.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in Source Repo, stacktrace=" + e.StackTrace);
                _logger.Error("Source Repo error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
        }

        public async Task<Source> UpdateSourceAsync(Source source)
        {
            if (!_context.Sources.Local.Any(c => c.Id == source.Id))
            {
                _context.Sources.Attach(source);
            }
            _context.Entry(source).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return source;
        }
    }
}
