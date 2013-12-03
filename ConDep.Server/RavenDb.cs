using System;
using ConDep.Dsl.Config;
using ConDep.Server.Api.Model;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Embedded;

namespace ConDep.Server
{
    public static class RavenDb
    {
        private static readonly Lazy<IDocumentStore> docStore = new Lazy<IDocumentStore>(() =>
            {
                var docStore = new EmbeddableDocumentStore
                    {
                        DataDirectory = "ConDepData",
                        UseEmbeddedHttpServer = true
                    };
                docStore.Conventions.RegisterIdConvention<ConDepEnvConfig>((dbname, commands, config) => "environments/" + config.EnvironmentName);
                docStore.Conventions.RegisterAsyncIdConvention<ConDepEnvConfig>((dbname, commands, config) => new CompletedTask<string>("environments/" + config.EnvironmentName));

                docStore.Conventions.RegisterIdConvention<QueueItem>((dbname, commands, item) => String.Format("execution_queue/{0}", item.ExecId));
                docStore.Conventions.RegisterAsyncIdConvention<QueueItem>((dbname, commands, item) => new CompletedTask<string>(String.Format("execution_queue/{0}", item.ExecId)));

                docStore.Conventions.RegisterIdConvention<ExecutionStatus>((dbname, commands, item) => "execution_status/" + item.ExecId);
                docStore.Conventions.RegisterAsyncIdConvention<ExecutionStatus>((dbname, commands, item) => new CompletedTask<string>("execution_status/" + item.ExecId));
                //OPTIONAL:
                //IndexCreation.CreateIndexes(typeof(Global).Assembly, docStore);

                return docStore;
            });

        public static IDocumentStore DocumentStore
        {
            get { return docStore.Value; }
        }
    }
}