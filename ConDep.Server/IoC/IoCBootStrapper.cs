using System.Collections.Generic;
using ConDep.Server.Domain.Infrastructure;
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
                }
            );
        }

        private IDocumentSession GetRavenSession()
        {
            return RavenDb.DocumentStore.OpenSession();
        }
    }
}