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
    public class SubjectRepository : ISubjectRepository
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
       
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());

        public async Task<Subject> AddSubectAsync(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject;
        }

        public async Task DeleteSubectAsync(long Id)
        {
            var subject = _context.Subjects.FirstOrDefault(c => c.Id == Id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Subject> GetSubectAsync(long id)
        {
            return _context.Subjects.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<List<Subject>> GetSubectsAsync()
        {
            try
            {
                return _context.Subjects.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in SubjectRepository, stacktrace=" + e.StackTrace);
                _logger.Error("SubjectRepository error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
        }

        public async Task<Subject> UpdateSubjectAsync(Subject subject)
        {
            if (!_context.Subjects.Local.Any(c => c.Id == subject.Id))
            {
                _context.Subjects.Attach(subject);
            }
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return subject;
        }
    }
}
