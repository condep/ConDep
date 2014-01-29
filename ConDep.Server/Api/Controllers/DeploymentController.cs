using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Remote.Node.Model;
using ConDep.Server.Commands;
using ConDep.Server.Infrastructure;

namespace ConDep.Server.Api.Controllers
{
    public class DeploymentController : ApiController
    {
        private readonly ICommandBus _bus;

        public DeploymentController(ICommandBus bus)
        {
            _bus = bus;
        }

        public HttpResponseMessage Post(string module, string artifact, string env)
        {
            var cmd = new QueueDeployment(module, artifact, env);
            _bus.Send(cmd);
            return CreateLinks(cmd.Id);
        }

        private HttpResponseMessage CreateLinks(Guid execId)
        {
            return Request.CreateResponse(
                HttpStatusCode.Created,
                new List<Link>
                    {
                        this.GetLinkFor<QueueController>(HttpMethod.Get),
                        this.GetLinkFor<LogController>(HttpMethod.Get, execId),
                        this.GetLinkFor<StatusController>(HttpMethod.Get, execId),
                        this.GetLinkFor<CancelController>(HttpMethod.Post, execId)
                    });
        }
    }
}