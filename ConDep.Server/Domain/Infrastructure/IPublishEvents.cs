using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace ConDep.Server.Domain.Infrastructure
{
    public interface IPublishEvents
    {
        [JsonIgnore]
        IEnumerable<IEvent> Events { get; }
        void ClearEvents();
        void AddEvent(IEvent @event);
    }
}