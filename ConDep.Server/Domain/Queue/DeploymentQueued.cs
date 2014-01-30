using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue
{
    public class DeploymentQueued : IEvent
    {

        public DeploymentQueued(Guid sourceId, string environment)
        {
            Environment = environment;
            SourceId = sourceId;
        }

        public string Environment { get; private set; }
        public Guid SourceId { get; private set; }
        public bool Dispatched { get; set; }
    }
}