using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Server.Model.DeploymentAggregate;

namespace ConDep.Server.Api.Controllers
{
    public class LogController : RavenDbController
    {
        public HttpResponseMessage Get(string id)
        {
            var item = Session.Load<Deployment>(RavenDb.GetFullId<Deployment>(id));

            if (item == null) return Request.CreateResponse(HttpStatusCode.NotFound);

            var relLogLoc = item.RelativeLogLocation;

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