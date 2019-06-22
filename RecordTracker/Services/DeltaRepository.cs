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
    public class DeltaRepository : IDeltaRepository
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();
  
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());

        public async Task<Delta> AddDeltaAsync(Delta delta)
        {
            _context.Deltas.Add(delta);
            await _context.SaveChangesAsync();
            return delta;
        }

        public async Task DeleteDeltaAsync(long Id)
        {
            var delta = _context.Deltas.FirstOrDefault(c => c.Id == Id);
            if (delta != null)
            {
                _context.Deltas.Remove(delta);

            }
            await _context.SaveChangesAsync();
        }

        public async Task<Delta> GetDeltaAsync(long id)
        {
            return await _context.Deltas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<List<Delta>> GetDeltasAsync()
        {
            try
            {
                return _context.Deltas.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in Delta Repo, stacktrace=" + e.StackTrace);
                _logger.Error("Delta Repo error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
        }

        public async Task<Delta> UpdateDeltaAsync(Delta delta)
        {
            if (!_context.Deltas.Local.Any(c => c.Id == delta.Id))
            {
                _context.Deltas.Attach(delta);
            }
            _context.Entry(delta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return delta;
        }
    }
}
