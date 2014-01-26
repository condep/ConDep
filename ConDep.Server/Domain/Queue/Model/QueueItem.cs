using System;

namespace ConDep.Server.Model.QueueAggregate
{
    public class QueueItem
    {
        public QueueItem(Guid deploymentId, string environment, string module, string artifact)
        {
            Id = deploymentId;
            CreatedUtc = DateTime.UtcNow;
            QueueStatus = QueueStatus.Waiting;
            Environment = environment;
            Module = module;
            Artifact = artifact;
        }

        public string Environment { get; private set; }
        public string Module { get; private set; }
        public string Artifact { get; private set; }

        public Guid Id { get; private set; }
        public DateTime CreatedUtc { get; private set; }
        public DateTime FinishedUtc { get; private set; }
        public QueueStatus QueueStatus { get; set; }
    }
}