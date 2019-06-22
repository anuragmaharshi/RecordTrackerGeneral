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
    public class GammaRepository : IGammaRepository
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());
        public async Task<Gamma> AddGammaAsync(Gamma gamma)
        {
            _context.Gammas.Add(gamma);
            await _context.SaveChangesAsync();
            return gamma;
        }

        public async Task DeleteGammaAsync(long Id)
        {
            var gamma = _context.Gammas.FirstOrDefault(c => c.Id == Id);
            if (gamma != null)
            {
                _context.Gammas.Remove(gamma);
            }
            await _context.SaveChangesAsync();
        }

        public Task<List<Gamma>> GetGammasAsync()
        {
            try
            {
                return _context.Gammas.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in GammaRepository, stacktrace=" + e.StackTrace);
                _logger.Error("GammaRepository error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
            
        }

        public Task<Gamma> GetGammaAsync(long id)
        {
            return _context.Gammas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Gamma> UpdateGammaAsync(Gamma gamma)
        {
            if (!_context.Gammas.Local.Any(c => c.Id == gamma.Id))
            {
                _context.Gammas.Attach(gamma);
            }
            _context.Entry(gamma).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return gamma;
        }
    }
}
