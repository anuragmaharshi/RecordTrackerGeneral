using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RecordTracker.SqliteDataLayer;
using NLog;

namespace RecordTracker.Services
{
    public class BetaRepository : IBetaRepository
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
      
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());
        public async Task<Beta> AddBetaAsync(Beta beta)
        {
            _context.Betas.Add(beta);
            await _context.SaveChangesAsync();
            return beta;
        }

        public async Task DeleteBetaAsync(long id)
        {
            var Beta = _context.Betas.FirstOrDefault(c => c.Id == id);
            if (Beta != null)
            {
                _context.Betas.Remove(Beta);

            }
            await _context.SaveChangesAsync();

        }

        public Task<Beta> GetBetaAsync(long id)
        {
            return _context.Betas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public  Task<List<Beta>> GetBetasAsync()
        {
            try
            {
                return _context.Betas.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in BetaRepository, stacktrace=" + e.StackTrace);
                _logger.Error("BetaRepository error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
           
        }

        public async Task<Beta> UpdateBetaAsync(Beta beta)
        {
            if (!_context.Betas.Local.Any(c => c.Id == beta.Id))
            {
                _context.Betas.Attach(beta);
            }
            _context.Entry(beta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return beta;
        }
    }
}
