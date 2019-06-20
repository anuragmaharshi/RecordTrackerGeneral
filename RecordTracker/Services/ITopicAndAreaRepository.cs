using RecordTracker.SqliteDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordTracker.Services
{
    public interface ITopicAndAreaRepository
    {
        Task<List<TopicsAndArea>> GetTopicAndAreasAsync();
        Task<TopicsAndArea> GetTopicAndAreaAsync(long id);
        Task<TopicsAndArea> AddTopicOrAreaAsync(TopicsAndArea topicOrArea);
        Task<TopicsAndArea> UpdateTopicAndAreaAsync(TopicsAndArea topicOrArea);
        Task DeleteTopicOrAreaAsync(long Id);
    }
}
