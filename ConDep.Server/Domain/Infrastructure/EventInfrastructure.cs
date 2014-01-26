using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;

namespace ConDep.Server.Infrastructure
{
    public interface IEvent
    {
        Guid SourceId { get; }
        bool Dispatched { get; set; }
    }

    public interface IHandleEvent
    {
    }

    public interface IHandleEvent<TEvent> : IHandleEvent
        where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
        void Publish<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent;
    }

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