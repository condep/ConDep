using System;
using ConDep.Server.Infrastructure;

namespace ConDep.Server.Commands
{
    public class ProcessEnvironmentQueue : ICommand
    {
        public ProcessEnvironmentQueue(string environment)
        {
            Id = Guid.NewGuid();
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; private set; }
    }

    public class ProcessDeploymentQueue : ICommand
    {
        public ProcessDeploymentQueue()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }

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