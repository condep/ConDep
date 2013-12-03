using System.Collections.Generic;
using System.IO;
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
            var logs = RavenDb.DocumentStore.DatabaseCommands.StartsWith("log", "", 0, 10, true);
            return logs.Select(log => new Link()
                {
                    Href = "/condepserver/api/" + log.Key,
                    Method = "GET",
                    Rel = "http://www.con-dep.net/rels/log"
                });
        }

        public HttpResponseMessage Get(string id)
        {
            var fileLoc = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), id + ".log");
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