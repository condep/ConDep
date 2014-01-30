using ConDep.Server.Application;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Domain.Queue;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;
using StructureMap;

namespace ConDep.Server.Tests
{
    [TestFixture]
    public class DeploymentProcessTests
    {
        private EmbeddableDocumentStore _store;

        [SetUp]
        public void Setup()
        {
            ObjectFactory.Configure(config =>
            {
                config.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                config.For<IEventBus>().Use<LocalEventBus>();
                config.For<ICommandBus>().Use<InMemoryCommandBus>();
                config.For<IDocumentStore>().Use(RavenDb.InMemoryDocumentStore);

            }
            );

            EventDispatcher.AutoRegister();
            RavenDb.InMemoryDocumentStore.Initialize();

            var bus = new LocalEventBus();
            CommandBus = new InMemoryCommandBus(bus);
            DeploymentProcess = new DeploymentProcess(CommandBus, new DeploymentScheduler(CommandBus));
        }

        [Test]
        public void TestThat_()
        {
            //var command = new CreateDeployment("someModule", "SomeArtifact", "SomeEnvironment");
            //var task = CommandBus.Send(command);
            //task.Wait();
        }

        [Test]
        public void TestThat_CommandQueuesDeployment()
        {
            var command = new QueueDeployment("someModule", "SomeArtifact", "SomeEnvironment");
        }

        protected ICommandBus CommandBus { get; set; }
        protected DeploymentProcess DeploymentProcess { get; set; }
    }
}