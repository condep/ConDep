using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Infrastructure;
using NUnit.Framework;
using FluentAssertions;
using Raven.Client;

namespace ConDep.Server.Tests
{
    public class given_event_dispatched
    {
        [Test]
        public void when_()
        {
            EventDispatcher.Regiser(new TestEventHandler());
            EventDispatcher.Dispatch(new TestEvent());
        }

        [Test]
        public void Test()
        {
            var handler = CommandHandlerContainer.Resolve<TestCommand>();
            handler.Execute(new TestCommand());
        }
    }

    public class TestCommand : ICommand
    {
        public Guid Id { get; private set; }
    }

    public class TestCommand2 : ICommand
    {
        public Guid Id { get; private set; }
    }

    public class TestCommandHandler : IHandleCommand<TestCommand>, IHandleCommand<TestCommand2>
    {
        public async Task<IAggregateRoot> Execute(TestCommand command)
        {
            command.Should().NotBeNull();
            return null;
        }

        public async Task<IAggregateRoot> Execute(TestCommand2 command)
        {
            throw new NotImplementedException();
        }

        public IDocumentSession Session { get; private set; }
        public IAggregateRoot Aggregate { get; private set; }
    }

    public class TestEventHandler : IHandleEvent<TestEvent>
    {
        public void Handle(TestEvent @event)
        {
            @event.Should().NotBeNull();
        }
    }

    public class TestEvent : IEvent
    {
        public Guid SourceId { get; private set; }
        public bool Dispatched { get; set; }
    }
}
