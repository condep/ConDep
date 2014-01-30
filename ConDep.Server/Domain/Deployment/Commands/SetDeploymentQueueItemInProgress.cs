using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Deployment.Commands
{
    public class SetDeploymentQueueItemInProgress : ICommand
    {
        public SetDeploymentQueueItemInProgress(Guid id, string environment)
        {
            Id = id;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; set; }
    }
}