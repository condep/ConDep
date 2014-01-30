using System;
using System.Collections.Generic;

namespace ConDep.Server.Domain.Infrastructure
{
    public class EventMessage
    {
        public EventMessage()
        {
            Id = Guid.NewGuid();
            Headers = new Dictionary<string, object>();
            Created = DateTime.UtcNow;
        }

        public EventMessage(Guid sourceId, Type type, string body) : this()
        {
            SourceId = sourceId;
            Body = body;
            Type = type;
        }

        public Guid Id { get; private set; }
        public Guid SourceId { get; private set; }
        public DateTime Created { get; private set; }
        public bool Dispatched { get; set; }
        public Dictionary<string, object> Headers { get; private set; }
        public string Body { get; private set; }
        public Type Type { get; private set; }
    }
}