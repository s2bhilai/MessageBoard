using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApp_MessageBoard.Data;

namespace MvcApp_MessageBoard.Controllers
{
    public class RepliesController : ApiController
    {
        public RepliesController(IMessageRepository repo)
        {
            _repo = repo;
        }

        public IMessageRepository _repo { get; set; }

        public IEnumerable<Reply> Get(int topicid)
        {
           return _repo.GetRepliesByTopic(topicid);
        }

        public HttpResponseMessage Post(int topicId,[FromBody]Reply newReply)
        {
            if (newReply.Created == default(DateTime))
            {
                newReply.Created = DateTime.UtcNow;
            }

            newReply.TopicId = topicId;

            if (_repo.AddReply(newReply) && _repo.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Created, newReply);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
