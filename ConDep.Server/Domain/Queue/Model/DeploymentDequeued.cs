using System;
using ConDep.Server.Infrastructure;

namespace ConDep.Server.Domain.Queue.Model
{
    public class DeploymentDequeued : IEvent
    {

        public DeploymentDequeued(Guid sourceId, string environment)
        {
            SourceId = sourceId;
            Environment = environment;
        }

        public Guid SourceId { get; private set; }
        public string Environment { get; private set; }
        public bool Dispatched { get; set; }
    }
}