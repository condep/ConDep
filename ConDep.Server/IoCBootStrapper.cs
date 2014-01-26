using System.Collections.Generic;
using ConDep.Server.Domain.Queue.Model;
using ConDep.Server.Infrastructure;
using Raven.Client;
using StructureMap;
using StructureMap.Pipeline;

namespace ConDep.Server
{
    public class IoCBootStrapper
    {
        public static void Bootstrap()
        {
           new IoCBootStrapper().Setup(); 
        }

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
                    config.For<ICommandBus>().Singleton().Use<InMemoryCommandBus>();
                    config.For<IDocumentStore>().Use(RavenDb.DocumentStore);
                    config.For<IDocumentSession>().LifecycleIs(new ThreadLocalStorageLifecycle()).AlwaysUnique().Use(GetRavenSession);
                    //config.For<DeploymentQueue>().Use(GetDeploymentQueue);
                }
            );
        }

        private IDocumentSession GetRavenSession()
        {
            return RavenDb.DocumentStore.OpenSession();
        }

        //private DeploymentQueue GetDeploymentQueue()
        //{
        //    var session = RavenDb.DocumentStore.OpenSession();
        //    var queue = session.Load<DeploymentQueue>(RavenDb.GetFullId<DeploymentQueue>());
        //    if(queue == null) queue = new DeploymentQueue(session);
        //    return queue;
        //}

        public static Container CreateIoCContainer()
        {
            return new Container(config =>
                {

                    config.For<ICommandBus>().Singleton().Use<InMemoryCommandBus>();
                    config.For<IEventBus>().Use<LocalEventBus>();
                    config.For<IDocumentStore>().Use(RavenDb.DocumentStore);
                });
        }
    }
}