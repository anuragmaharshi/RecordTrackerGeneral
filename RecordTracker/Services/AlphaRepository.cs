using NLog;
using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public class AlphaRepository : IAlphaRepository
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        //DataLayerContext _context = new DataLayerContext(@"C:\Users\Home\MainApplication.db");
        //DataLayerContext _context = new DataLayerContext();
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());
        public async Task<Alpha> AddAlphaAsync(Alpha alpha) 
        {
            
            _context.Alphas.Add(alpha);
            await _context.SaveChangesAsync();
            return alpha;
        }

        public async Task DeleteAlphaAsync(long Id)
        {
            var alpha= _context.Alphas.FirstOrDefault(c => c.Id == Id);
            if (alpha != null)
            {
                _context.Alphas.Remove(alpha);

            }
            await _context.SaveChangesAsync();
        }

        public Task<Alpha> GetAlphaAsync(long id)
        {
            return _context.Alphas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<List<Alpha>> GetAlphasAsync()
        {
            try
            {
                return _context.Alphas.ToListAsync();
            }
            catch(Exception e)
            {
                _logger.Error("Some error have occured in AlphaRepository, stacktrace=" + e.StackTrace);
                _logger.Error("AlphaRepository error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
            
        }

        public async Task<Alpha> UpdateAlphaAsync(Alpha alpha)
        {
            if (!_context.Alphas.Local.Any(c => c.Id == alpha.Id))
            {
                _context.Alphas.Attach(alpha);
            }
            _context.Entry(alpha).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return alpha;
        }
    }
}
