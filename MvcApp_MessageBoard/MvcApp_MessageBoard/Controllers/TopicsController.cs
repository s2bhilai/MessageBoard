using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApp_MessageBoard.Data;

namespace MvcApp_MessageBoard.Controllers
{
    public class TopicsController : ApiController
    {

        public TopicsController(IMessageRepository repo)
        {
            _repo = repo;
        }

        public IMessageRepository _repo { get; set; }

        public IEnumerable<Topic> Get(bool includeReplies=false)
        {
            IQueryable<Topic> results;

            if (includeReplies == true)
            {
                results = _repo.GetTopicsIncludingReplies();
            }
            else
            {
                results = _repo.GetTopics();
            }
             
            var topics=results
                .OrderByDescending(t => t.Created)
                .Take(50).ToList();

            return topics;
        }

        public HttpResponseMessage Post([FromBody]Topic newTopic)
        {
            if (newTopic.Created == default(DateTime))
            {
                newTopic.Created = DateTime.UtcNow;
            }

            if (_repo.AddTopic(newTopic) && _repo.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Created,newTopic);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
