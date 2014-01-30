using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Events
{
    public class DeploymentCancelled : IEvent
    {
        public DeploymentCancelled(Guid sourceId)
        {
            SourceId = sourceId;
        }

        public Guid SourceId { get; private set; }
        public bool Dispatched { get; set; }
    }
}