using System;
using System.Net;
using System.Net.Http;
using ConDep.Server.Domain.Deployment;
using ConDep.Server.Domain.Deployment.Model;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Api.Controllers
{
    public class CancelController : RavenDbController    {
        private readonly ICommandBus _bus;

        public CancelController(ICommandBus bus)
        {
            _bus = bus;
        }

        public HttpResponseMessage Post(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new ConDepDeploymentNotFound());
            }

            var responseMessage = Validate(guid);
            if (responseMessage.IsSuccessStatusCode)
            {
                var cmd = new CancelDeployment(guid);
                _bus.Send(cmd);
            }
            return responseMessage;
         }

        private HttpResponseMessage Validate(Guid id)
        {
            var info = Session.Load<Deployment>(id);

            if (info == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No execution with id " + id + " exist.");
            if (info.Status != DeploymentStatus.InProgress) return Request.CreateErrorResponse(HttpStatusCode.Gone, string.Format("No execution in progress with id {0} found. Status of execution is {1}.", id, info.Status));
                
            return Request.CreateResponse(HttpStatusCode.Created, new[]
                {
                    this.GetLinkFor<StatusController>(HttpMethod.Get, id),
                    this.GetLinkFor<LogController>(HttpMethod.Get, id),
                    this.GetLinkFor<QueueController>(HttpMethod.Get, id)
                });
        }
    }
}