using System;
using ConDep.Server.Infrastructure;

namespace ConDep.Server.DomainEvents
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