using System;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.QueueAggregate;

namespace ConDep.Server.Domain.Queue.Model
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