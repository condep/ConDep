using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Remote.Node.Model;
using ConDep.Server.Api.Model;

namespace ConDep.Server.Api.Controllers
{
    public class LogsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return GetLinksToEnvironments();
        }

        public HttpResponseMessage Get(string env, string id = null)
        {
            if (string.IsNullOrWhiteSpace(id)) return GetLinksToLogs(env);

            return GetLogById(id);
        }

        private HttpResponseMessage GetLinksToLogs(string env)
        {
            var logs = new List<Link>();
            using(var session = RavenDb.DocumentStore.OpenSession())
            {
                var items = session.Query<ExecutionStatus, ExecutionStatus_ByEnvironment>()
                                   .Where(x => x.Environment.Equals(env, StringComparison.InvariantCultureIgnoreCase));

                foreach (var item in items)
                {
                    logs.Add(new Link
                    {
                        Href = string.Format("/condepserver/api/logs/{0}/{1}", item.Environment, item.ExecId),
                        Method = "GET",
                        Rel = "http://www.con-dep.net/rels/server/logs"
                    });
                }
            }

            return Request.CreateResponse(HttpStatusCode.Found, logs);
        }

        private HttpResponseMessage GetLinksToEnvironments()
        {
            return Request.CreateResponse(HttpStatusCode.Found, "Should return links to all evnironments");
        }

        private HttpResponseMessage GetLogById(string id)
        {
            string relLogLoc = "";
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                var item = session.Load<ExecutionStatus>(RavenDb.GetFullId<ExecutionStatus>(id));

                if (item == null) return Request.CreateResponse(HttpStatusCode.NotFound);

                relLogLoc = item.RelativeLogLocation;
            }

            var fileLoc = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), relLogLoc);
            if (!File.Exists(fileLoc))
                return Request.CreateResponse(HttpStatusCode.NotFound);

            using (var fileStream = new FileStream(fileLoc, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var textReader = new StreamReader(fileStream))
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(textReader.ReadToEnd());
                    return response;
                }
            }
        }
    }
}