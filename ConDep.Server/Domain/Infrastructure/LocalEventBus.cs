using System.Collections.Generic;

namespace ConDep.Server.Domain.Infrastructure
{
    public class LocalEventBus : IEventBus
    {
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            EventDispatcher.Dispatch(@event);
        }

        public void Publish<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                Publish(@event);
            }
        }

    }
}