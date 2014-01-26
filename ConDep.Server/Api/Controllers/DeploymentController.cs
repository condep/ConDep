using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Remote.Node.Model;
using ConDep.Server.Commands;
using ConDep.Server.Infrastructure;
using Raven.Client;

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
                        this.GetLinkFor<QueueController>(HttpMethod.Get, execId),
                        this.GetLinkFor<LogController>(HttpMethod.Get, execId),
                        this.GetLinkFor<StatusController>(HttpMethod.Get, execId),
                        this.GetLinkFor<CancellaionController>(HttpMethod.Get, execId)
                    });
        }

        private static void CreateExecutionStatus(string module, string env, Guid execId, IDocumentSession session)
        {
            //var executionInfo = new ExecutionInfo()
            //    {
            //        Status = ExecutionStatus.Queued,
            //        ExecId = execId,
            //        StartedUtc = DateTime.UtcNow,
            //        Environment = env,
            //        Module = module,
            //        RelativeLogLocation = string.Format("{0}/{1}/{2}", "logs", env, execId + ".log")
            //    };
            //session.Store(executionInfo);
        }
    }
}