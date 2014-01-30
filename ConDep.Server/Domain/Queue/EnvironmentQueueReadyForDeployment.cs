using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue
{
    public class EnvironmentQueueReadyForDeployment : IEvent
    {
        public EnvironmentQueueReadyForDeployment(Guid id, string environment, string module, string artifact)
        {
            SourceId = id;
            Environment = environment;
            Module = module;
            Artifact = artifact;
        }

        public Guid SourceId { get; private set; }
        public string Environment { get; private set; }
        public string Module { get; private set; }
        public string Artifact { get; private set; }
        public bool Dispatched { get; set; }
    }
}