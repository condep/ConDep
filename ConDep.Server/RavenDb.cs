using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Server.Api.Model;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace ConDep.Server
{
    public static class RavenDb
    {
        private static readonly Dictionary<Type, string> _idTemplates = new Dictionary<Type, string>
            {
                {
                    typeof (ExecutionStatus),
                    "execution_status/{0}"
                },
                {
                    typeof(QueueItem),
                    "execution_queue/{0}"
                },
                {
                    typeof(ConDepEnvConfig),
                    "environments/{0}"
                }
            };

        private static readonly Lazy<IDocumentStore> docStore = new Lazy<IDocumentStore>(() =>
            {
                var docStore = new EmbeddableDocumentStore
                    {
                        DataDirectory = "ConDepData",
                        UseEmbeddedHttpServer = true
                    };
                docStore.Conventions.RegisterIdConvention<ConDepEnvConfig>((dbname, commands, config) => GetFullId<ConDepEnvConfig>(config.EnvironmentName));
                docStore.Conventions.RegisterAsyncIdConvention<ConDepEnvConfig>((dbname, commands, config) => new CompletedTask<string>(GetFullId<ConDepEnvConfig>(config.EnvironmentName)));

                docStore.Conventions.RegisterIdConvention<QueueItem>((dbname, commands, item) => GetFullId<QueueItem>(item.ExecId));
                docStore.Conventions.RegisterAsyncIdConvention<QueueItem>((dbname, commands, item) => new CompletedTask<string>(GetFullId<QueueItem>(item.ExecId)));

                docStore.Conventions.RegisterIdConvention<ExecutionStatus>((dbname, commands, item) => GetFullId<ExecutionStatus>(item.ExecId));
                docStore.Conventions.RegisterAsyncIdConvention<ExecutionStatus>((dbname, commands, item) => new CompletedTask<string>(GetFullId<ExecutionStatus>(item.ExecId)));

                return docStore;
            });

        public static IDocumentStore DocumentStore
        {
            get { return docStore.Value; }
        }

        public static void CreateIndexes()
        {
            IndexCreation.CreateIndexes(typeof(RavenDb).Assembly, DocumentStore);
        }

        public static string GetFullId<T>(string id)
        {
            return string.Format(_idTemplates[typeof(T)], id);
        }
    }

    public class ExecutionStatus_ByEnvironment : AbstractIndexCreationTask<ExecutionStatus>
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
}