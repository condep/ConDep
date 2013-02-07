using System;
using ConDep.WebQ.Client;
using NUnit.Framework;

namespace ConDep.WebQ.Tests
{
    [TestFixture]
    public class WebQTests
    {
        [Test]
        public void TestThatQueueIsCorrectlyProcessed()
        {
            const int startPosition = 5;
            var currentPosition = startPosition;
            var client = new WebQMockClient(startPosition);
            var queue = new WebQueue(client, "test", TimeSpan.FromMilliseconds(10));
            queue.WebQueuePositionUpdate += (sender, args) => Assert.That(currentPosition--, Is.EqualTo(args.Item.Position));
            queue.WaitInQueue(TimeSpan.FromMinutes(1));
        }

        [Test]
        [ExpectedException(typeof(TimeoutException))]
        public void TestThatQueueTimesOutCorrectly()
        {
            const int startPosition = 5;
            var client = new WebQMockClient(startPosition);
            var queue = new WebQueue(client, "test", TimeSpan.FromMilliseconds(10));
            queue.WaitInQueue(TimeSpan.FromMilliseconds(1));
        }

        [Test]
        public void TestThatQueueReturnsImmidiatlyWhenAddedToEmptyQueue()
        {
            const int startPosition = 0;
            var client = new WebQMockClient(startPosition);
            var queue = new WebQueue(client, "test", TimeSpan.FromMilliseconds(10));
            queue.WebQueuePositionUpdate += (sender, args) => Assert.That(args.Item.Position, Is.EqualTo(0));
            queue.WaitInQueue(TimeSpan.FromMilliseconds(1));
        }

        [Test]
        public void TestThatTimeOutAlertsGetsCalled()
        {
            const int startPosition = 5;
            var client = new WebQMockClient(startPosition);
            var queue = new WebQueue(client, "test", TimeSpan.FromMilliseconds(10));
            queue.WebQueueTimeoutUpdate += (sender, args) => Assert.That(args.Message, Is.Not.Null.Or.Empty);
            queue.WaitInQueue(TimeSpan.FromMinutes(1));
        }
    }
}