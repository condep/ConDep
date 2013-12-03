using System;

namespace ConDep.Server.Api.Model
{
    public class QueueItem
    {
        public string ExecId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime FinishedUtc { get; set; }
        public QueueExecutionData ExecutionData { get; set; }
        public QueueStatus QueueStatus { get; set; }
    }

    public enum QueueStatus
    {
        NotSet,
        InQueue,
        InProgress,
        Finished
    }

    public class QueueExecutionData
    {
        public string Module { get; set; }
        public string Artifact { get; set; }
        public string Environment { get; set; }
    }
}