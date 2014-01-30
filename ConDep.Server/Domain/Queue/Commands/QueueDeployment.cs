using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue.Commands
{
    public class QueueDeployment : ICommand
    {
        public QueueDeployment(string module, string artifact, string environment)
        {
            Id = Guid.NewGuid();
            Module = module;
            Artifact = artifact;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Module { get; private set; }
        public string Artifact { get; private set; }
        public string Environment { get; private set; }
    }
}