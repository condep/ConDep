using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Infrastructure;
using NUnit.Framework;

namespace ConDep.Server.Tests
{
    public abstract class AggregateRootTestFixture<T> where T : IAggregateRoot
    {
        private bool _caughtAccessed;
        private Exception _caught;
        protected T Sut;
        protected IEnumerable<object> Events;
        protected Guid AggregateRootId = Guid.NewGuid();

        public Exception Caught
        {
            get
            {
                _caughtAccessed = true;
                return _caught;
            }
        }

        protected abstract T New();
        protected virtual void Before() { }
        protected abstract IEnumerable<object> Given();
        protected abstract void When();

        [TestFixtureSetUp]
        public void Setup()
        {
            _caught = null;

            Before();
            var given = Given().ToList();

            Sut = New();

            foreach (var e in given)
            {
                var unwrappedEvent = UnwrapEventBuilder(e);

                //var domainEvent = unwrappedEvent as DomainEvent;
                //if (domainEvent != null && domainEvent.Audit == null)
                //    domainEvent.Audit = new Audit("104171abcd@Z63", "");

                //((IPublishEvents)Sut).AddEvent(domainEvent ?? unwrappedEvent);
            }

            try
            {
                When();
                Events = ((IPublishEvents)Sut).Events;
            }
            catch (Exception e)
            {
                _caught = e;
            }
        }

        private static object UnwrapEventBuilder(object e)
        {
            var @event = e;
            //if (e.GetType().Name == typeof(EventBuilder<>).Name)
            //{
            //    @event = e.GetType().InvokeMember("ToEvent", BindingFlags.InvokeMethod, null, e, null);
            //}
            return @event;
        }

        protected virtual void After() { }

        [TestFixtureTearDown]
        public void TearDown()
        {
            After();
        }

        [TearDown]
        public void Dispose()
        {
            if (_caught != null && !_caughtAccessed)
            {
                throw _caught;
            }
        }
    }
}