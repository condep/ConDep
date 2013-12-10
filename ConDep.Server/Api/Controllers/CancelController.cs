using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConDep.Server.Api.Controllers
{
    public class CancelController : ApiController    {
         public HttpResponseMessage Post(string id)
         {
            ConDepServer.QueueExecutor.Cancel(id);
            return Request.CreateResponse(HttpStatusCode.Found, true);
         }
    }
}