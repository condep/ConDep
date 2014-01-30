using System;
using System.Collections.Generic;

namespace ConDep.Server.Domain.Infrastructure
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
}