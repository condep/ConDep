using System;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Domain.Queue.Model;

namespace ConDep.Server.Domain.Queue
{
    public class DeploymentQueueItemInProgress : IEvent
    {
        public DeploymentQueueItemInProgress(Guid id, string environment)
        {
            SourceId = id;
            Environment = environment;
            NewStatus = QueueStatus.DeploymentInProgress;
        }

        public Guid SourceId { get; private set; }
        public string Environment { get; private set; }
        public QueueStatus OldStatus { get; private set; }
        public QueueStatus NewStatus { get; private set; }
        public bool Dispatched { get; set; }
    }
}