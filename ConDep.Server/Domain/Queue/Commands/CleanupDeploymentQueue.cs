using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue.Commands
{
    public class CleanupDeploymentQueue : ICommand
    {
        public CleanupDeploymentQueue(TimeSpan anythingOlderThan)
        {
            AnythingOlderThan = anythingOlderThan;
            Id = Guid.NewGuid();
        }

        public TimeSpan AnythingOlderThan { get; private set; }
        public Guid Id { get; private set; }
    }
}