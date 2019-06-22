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
    public class ThetaRepository : IThetaRepository
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
       
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());

        public async Task<Theta> AddThetaAsync(Theta theta)
        {
            _context.Thetas.Add(theta);
            await _context.SaveChangesAsync();
            return theta;
        }

        public async Task DeleteThetaAsync(long Id)
        {
            var theta = _context.Thetas.FirstOrDefault(c => c.Id == Id);
            if (theta != null)
            {
                _context.Thetas.Remove(theta);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Theta> GetThetaAsync(long id)
        {
            return _context.Thetas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<List<Theta>> GetThetasAsync()
        {
            try
            {
                return _context.Thetas.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in ThetaRepository, stacktrace=" + e.StackTrace);
                _logger.Error("ThetaRepository error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
        }

        public async Task<Theta> UpdateThetaAsync(Theta theta)
        {
            if (!_context.Thetas.Local.Any(c => c.Id == theta.Id))
            {
                _context.Thetas.Attach(theta);
            }
            _context.Entry(theta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return theta;
        }
    }
}
