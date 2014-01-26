using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Server.Domain.Queue.Model;
using ConDep.Server.Model.DeploymentAggregate;
using ConDep.Server.Model.QueueAggregate;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace ConDep.Server
{
    public static class RavenDb
    {
        public static int NumberOfConcurrencyExceptions;
        private static readonly Dictionary<Type, string> _idTemplates = new Dictionary<Type, string>
            {
                {
                    typeof (Deployment),
                    "execution_info/{0}"
                },
                {
                    typeof(QueueItem),
                    "execution_queue/{0}"
                },
                {
                    typeof(ConDepEnvConfig),
                    "environments/{0}"
                },
                {
                    typeof(EnvironmentQueue),
                    "deployment_queue/{0}"
                },
                {
                    typeof(DeploymentQueue),
                    "deployment_queue"
                }
            };

        private static readonly Lazy<IDocumentStore> docStore = new Lazy<IDocumentStore>(() =>
            {
                var docStore = new EmbeddableDocumentStore
                    {
                        DataDirectory = "ConDepData",
                        UseEmbeddedHttpServer = true
                    };
                RegisterConventions(docStore);

                return docStore;
            });

        private static void RegisterConventions(EmbeddableDocumentStore docStore)
        {
            docStore.Conventions.RegisterIdConvention<ConDepEnvConfig>(
                (dbname, commands, config) => GetFullId<ConDepEnvConfig>(config.EnvironmentName));
            docStore.Conventions.RegisterAsyncIdConvention<ConDepEnvConfig>(
                (dbname, commands, config) => new CompletedTask<string>(GetFullId<ConDepEnvConfig>(config.EnvironmentName)));

            docStore.Conventions.RegisterIdConvention<QueueItem>(
                (dbname, commands, item) => GetFullId<QueueItem>(item.Id.ToString()));
            docStore.Conventions.RegisterAsyncIdConvention<QueueItem>(
                (dbname, commands, item) => new CompletedTask<string>(GetFullId<QueueItem>(item.Id.ToString())));

            docStore.Conventions.RegisterIdConvention<Deployment>(
                (dbname, commands, item) => GetFullId<Deployment>(item.Id.ToString()));
            docStore.Conventions.RegisterAsyncIdConvention<Deployment>(
                (dbname, commands, item) => new CompletedTask<string>(GetFullId<Deployment>(item.Id.ToString())));

            docStore.Conventions.RegisterIdConvention<EnvironmentQueue>(
                (dbname, commands, item) => item.Environment);
            docStore.Conventions.RegisterAsyncIdConvention<EnvironmentQueue>(
                (dbname, commands, item) => new CompletedTask<string>(item.Environment));

            docStore.Conventions.RegisterIdConvention<DeploymentQueue>(
                (dbname, commands, item) => GetFullId<DeploymentQueue>());
            docStore.Conventions.RegisterAsyncIdConvention<EnvironmentQueue>(
                (dbname, commands, item) => new CompletedTask<string>(GetFullId<DeploymentQueue>()));
        }

        private static readonly Lazy<IDocumentStore> inMemoryDocStore = new Lazy<IDocumentStore>(() =>
        {
            var docStore = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                UseEmbeddedHttpServer = true
            };
            RegisterConventions(docStore);

            return docStore;
        });

        public static IDocumentStore DocumentStore
        {
            get { return docStore.Value; }
        }

        public static IDocumentStore InMemoryDocumentStore
        {
            get { return inMemoryDocStore.Value; }
        }

        public static void CreateIndexes()
        {
            IndexCreation.CreateIndexes(typeof(RavenDb).Assembly, DocumentStore);
        }

        public static string GetFullId<T>(string id)
        {
            return string.Format(_idTemplates[typeof(T)], id);
        }

        public static string GetFullId<T>()
        {
            return _idTemplates[typeof(T)];
        }
    }

    public class EnvironmentQueue_Environments : AbstractIndexCreationTask<EnvironmentQueue, EnvironmentQueue_Environments.Result>
    {
        public class Result
        {
            public string Environment { get; set; }
        }

        public EnvironmentQueue_Environments()
        {
            Map = items => from item in items
                           select new {item.Environment};
        }
    }
    public class ExecutionStatus_ByEnvironment : AbstractIndexCreationTask<Deployment>
    {
        public ExecutionStatus_ByEnvironment()
        {
            Map = items => from item in items
                           select new
                               {    
                                   item.Environment,
                                   item.StartedUtc
                               };
        }
    }

    //public class QueueItem_ByEnvironmentAndStatus : AbstractIndexCreationTask<QueueItem>
    //{
    //    public QueueItem_ByEnvironmentAndStatus()
    //    {
    //        //throw new NotImplementedException();
    //        //Map = items => from item in items
    //        //               select new
    //        //                   {
    //        //                       ExecutionData_Environment = item.ExecutionData.Environment,
    //        //                       item.ExecutionData.Environment,
    //        //                       item.QueueStatus,
    //        //                       item.CreatedUtc
    //        //                   };
    //    }
    //}
}