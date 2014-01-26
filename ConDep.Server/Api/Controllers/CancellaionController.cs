using System.Net;
using System.Net.Http;
using ConDep.Server.Model.DeploymentAggregate;

namespace ConDep.Server.Api.Controllers
{
    public class CancellaionController : RavenDbController    {
        public HttpResponseMessage Post(string id)
        {
            var responseMessage = Validate(id);
            if (responseMessage.IsSuccessStatusCode)
            {
                //CommandHandlerContainer.QueueExecutor.Cancel(id);
            }
            return responseMessage;
         }

        private HttpResponseMessage Validate(string id)
        {
            var info = Session.Load<Deployment>(RavenDb.GetFullId<Deployment>(id));

            if (info == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No execution with id " + id + " exist.");
            if (info.Status == DeploymentStatus.InProgress) return Request.CreateErrorResponse(HttpStatusCode.Gone, string.Format("No execution in progress with id {0} found. Status of execution is {1}.", id, info.Status));
                
            return Request.CreateResponse(HttpStatusCode.Created, new[]
                {
                    this.GetLinkFor<StatusController>(HttpMethod.Get, id),
                    this.GetLinkFor<LogController>(HttpMethod.Get, id),
                    this.GetLinkFor<QueueController>(HttpMethod.Get, id)
                });
        }
    }
}