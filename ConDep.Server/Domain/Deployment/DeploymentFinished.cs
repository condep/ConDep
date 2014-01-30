using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment
{
    public class DeploymentFinished : IEvent
    {
        public DeploymentFinished(Guid id, string environment)
        {
            SourceId = id;
            Environment = environment;
        }

        public string Environment { get; set; }

        public Guid SourceId { get; private set; }
        public bool Dispatched { get; set; }
    }
}