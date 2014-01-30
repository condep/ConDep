using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue.Commands
{
    public class ProcessDeploymentQueue : ICommand
    {
        public ProcessDeploymentQueue()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}