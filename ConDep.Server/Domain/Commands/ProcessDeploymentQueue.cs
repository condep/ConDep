using System;
using ConDep.Server.Infrastructure;

namespace ConDep.Server.Commands
{
    public class ProcessDeploymentQueue : ICommand
    {
        public ProcessDeploymentQueue()
        {
        }

        public ProcessDeploymentQueue(string environment)
        {
            Environment = environment;
        }

        public Guid Id { get; private set; }
        public string Environment { get; private set; }
    }

    public class ProcessQueueItem : ICommand
    {
        public ProcessQueueItem(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
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