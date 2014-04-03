using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp_MessageBoard.Data
{
    public interface IMessageRepository
    {
        IQueryable<Topic> GetTopics();
        IQueryable<Reply> GetRepliesByTopic(int topicId);
        IQueryable<Topic> GetTopicsIncludingReplies();

        bool Save();
        bool AddTopic(Topic newTopic);

        bool AddReply(Reply newReply);
    }
}