using System;
using ConDep.WebQ.Client;
using NUnit.Framework;

namespace ConDep.WebQ.Tests
{
    [TestFixture]
    public class WebQTests
    {
        [Test]
        [Ignore]
        public void TestThat()
        {
            var proxy = new Client.Client(new Uri("http://localhost/ConDepWebQ/"));
            var item = proxy.Enqueue("Test");
            Assert.That(item.Id, Is.Not.Null.Or.Empty);
            Assert.That(item.Position, Is.EqualTo(0));
        }
         
    }
}