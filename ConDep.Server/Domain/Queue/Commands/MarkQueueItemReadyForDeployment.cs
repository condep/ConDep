using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue.Commands
{
    public class MarkQueueItemReadyForDeployment : ICommand
    {
        public MarkQueueItemReadyForDeployment(Guid id, string environment)
        {
            Id = id;
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; private set; }
    }
}