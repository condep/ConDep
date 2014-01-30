using System;
using ConDep.Server.Domain.Infrastructure;

namespace ConDep.Server.Domain.Queue
{
    public class QueueItemExpired : IEvent
    {
        public QueueItemExpired(Guid id, string environment, DateTime createdUtc)
        {
            SourceId = id;
            Environment = environment;
            CreatedUtc = createdUtc;
            ExpiredUtc = DateTime.UtcNow;
        }

        public DateTime ExpiredUtc { get; private set; }
        public Guid SourceId { get; private set; }
        public string Environment { get; private set; }
        public DateTime CreatedUtc { get; private set; }
        public bool Dispatched { get; set; }
    }
}