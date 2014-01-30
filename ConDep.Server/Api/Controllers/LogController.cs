using System;
using System.IO;
using System.Net;
using System.Net.Http;
using ConDep.Server.Domain.Deployment.Model;

namespace ConDep.Server.Api.Controllers
{
    public class LogController : RavenDbController
    {
        public HttpResponseMessage Get(Guid id)
        {
            var item = Session.Load<Deployment>(id);

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