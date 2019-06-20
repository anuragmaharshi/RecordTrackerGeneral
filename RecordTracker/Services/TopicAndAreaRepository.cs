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
    public class TopicAndAreaRepository : ITopicAndAreaRepository
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        //DataLayerContext _context = new DataLayerContext(@"C:\Users\Home\MainApplication.db");
        //DataLayerContext _context = new DataLayerContext();
        DataLayerContext _context = new DataLayerContext(Constants.GetDbFilePath());
        public async Task<TopicsAndArea> AddTopicOrAreaAsync(TopicsAndArea topicOrArea)
        {
            _context.TopicsAndAreas.Add(topicOrArea);
            await _context.SaveChangesAsync();
            return topicOrArea;
        }

        public async Task DeleteTopicOrAreaAsync(long Id)
        {
            var topicAndArea = _context.TopicsAndAreas.FirstOrDefault(c => c.Id == Id);
            if (topicAndArea != null)
            {
                _context.TopicsAndAreas.Remove(topicAndArea);
            }
            await _context.SaveChangesAsync();
        }

        public Task<List<TopicsAndArea>> GetTopicAndAreasAsync()
        {
            try
            {
                return _context.TopicsAndAreas.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.Error("Some error have occured in TopicAndAreaRepository, stacktrace=" + e.StackTrace);
                _logger.Error("TopicAndAreaRepository error message is " + e.Message + " inner error is " + e.InnerException.Message);
                return null;
            }
            
        }

        public Task<TopicsAndArea> GetTopicAndAreaAsync(long id)
        {
            return _context.TopicsAndAreas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<TopicsAndArea> UpdateTopicAndAreaAsync(TopicsAndArea topicOrArea)
        {
            if (!_context.TopicsAndAreas.Local.Any(c => c.Id == topicOrArea.Id))
            {
                _context.TopicsAndAreas.Attach(topicOrArea);
            }
            _context.Entry(topicOrArea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return topicOrArea;
        }
    }
}
