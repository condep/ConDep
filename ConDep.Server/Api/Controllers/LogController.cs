using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Remote.Node.Model;

namespace ConDep.Server.Api.Controllers
{
    public class LogController : ApiController
    {
        public IEnumerable<Link> Get()
        {
            var logs = ConDepServer.DocumentStore.DatabaseCommands.StartsWith("log", "", 0, 10, true);
            return logs.Select(log => new Link()
                {
                    Href = "/condepserver/api/" + log.Key,
                    Method = "GET",
                    Rel = "http://www.con-dep.net/rels/log"
                });
        }

        public ExecutionLog Get(string logId)
         {
             ExecutionLog log;
             using (var session = ConDepServer.DocumentStore.OpenSession())
             {
                 log = session.Load<ExecutionLog>("log/" + logId);
             }

             if (log == null)
             {
                 throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, string.Format("Log with id {0} not found.", logId)));
             }

             return log;
         }
    }
}