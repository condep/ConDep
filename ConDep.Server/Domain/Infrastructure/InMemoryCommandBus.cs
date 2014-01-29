using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConDep.Server.Domain.Infrastructure;
using Newtonsoft.Json;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace ConDep.Server.Infrastructure
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly IEventBus _eventBus;
        private readonly InMemoryCommandQueue _queue;

        public InMemoryCommandBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _queue = new InMemoryCommandQueue();
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            Trace.WriteLine("Resolving handler for " + typeof(TCommand).Name);
            var handler = CommandHandlerContainer.Resolve<TCommand>();
            Trace.WriteLine("Handler " + handler.GetType().Name + "found for command " + typeof(TCommand).Name);

            IAggregateRoot aggregate = handler.Execute(command).Result;

            PersistChanges(handler, aggregate);
            //_queue.Enqueue(HandleCommand, handler, command);
            //Trace.WriteLine("Queuing command " + typeof(TCommand).Name + " for execution");
        }

        private async void HandleCommand<TCommand>(IHandleCommand<TCommand> handler, TCommand command) where TCommand : ICommand
        {
            Trace.WriteLine("Executing command " + typeof(TCommand).Name + "...");
            var aggregate = await handler.Execute(command);
            Trace.WriteLine("Persisting changes for command " + typeof(TCommand).Name + "...");
            PersistChanges(handler, aggregate);
        }

        private void PersistChanges(IHandleCommand handler, IAggregateRoot aggregate)
        {
            var eventPublisher = aggregate as IPublishEvents;
            using (var session = handler.Session)
            {
                if (!session.Advanced.HasChanges) return;

                if (eventPublisher != null)
                {
                    StoreEvents(session, eventPublisher.Events);
                }
                try
                {
                    session.Advanced.UseOptimisticConcurrency = true;
                    session.SaveChanges();
                }
                catch (ConcurrencyException concurrencyException)
                {
                    Trace.WriteLine("Concurrency exception: " + concurrencyException.Message);
                }
            }

            if (eventPublisher != null)
            {
                Trace.WriteLine("Publishing events in entity " + eventPublisher.GetType().Name + "...");

                _eventBus.Publish(eventPublisher.Events);
                eventPublisher.ClearEvents();
            }
        }

        private void StoreEvents(IDocumentSession session, IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                string serializeObject = JsonConvert.SerializeObject(@event);
                var message = new EventMessage(@event.SourceId, @event.GetType(), serializeObject) { Dispatched = true };
                session.Store(message);
            }
        }
    }
}