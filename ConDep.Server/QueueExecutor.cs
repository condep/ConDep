using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Config;
using ConDep.Server.Api.Model;
using ConDep.Server.Execution;
using Raven.Client;
using Raven.Client.Linq;

namespace ConDep.Server
{
    public class QueueExecutor
    {
        private readonly EventLog _eventLog;
        private readonly Dictionary<string, QueueItem> _itemsInExecution = new Dictionary<string, QueueItem>();
        private List<CancellationTokenSource> _tokenSources = new List<CancellationTokenSource>();
 
        public QueueExecutor(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        protected void Execute(string id)
        {
            var tokenSource = new CancellationTokenSource();
            _tokenSources.Add(tokenSource);

            _eventLog.WriteEntry(string.Format("A new execution was added to the queue with id {0}.", id), EventLogEntryType.Information);
            AppDomain appDomain = null;
            try
            {
                appDomain = AppDomain.CreateDomain("ConDepExec", null, AppDomain.CurrentDomain.SetupInformation);

                var executorCancelable = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);
                tokenSource.Token.Register(executorCancelable.Cancel);

                var executor = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);

                ConDepSettings settings;
                QueueItem item;

                using (var session = RavenDb.DocumentStore.OpenSession())
                {
                    item = session.Load<QueueItem>(id);
                    item.QueueStatus = QueueStatus.InProgress;

                    settings = new ConDepSettings
                    {
                        Options = new ConDepOptions
                        {
                            //Assembly = LoadAssembly(module),
                            Application = item.ExecutionData.Artifact,
                            Environment = item.ExecutionData.Environment
                        },
                        Config = session.Load<ConDepEnvConfig>("environments/" + item.ExecutionData.Environment)
                    };

                    foreach (var server in settings.Config.Servers.Where(server => !server.DeploymentUser.IsDefined()))
                    {
                        server.DeploymentUser = settings.Config.DeploymentUser;
                    }

                    session.SaveChanges();
                }

                executor.Execute(executorCancelable, item.ExecId, settings, item.ExecutionData.Module, item.ExecutionData.Artifact, item.ExecutionData.Environment);
            }
            finally
            {
                if (appDomain != null)
                    AppDomain.Unload(appDomain);

                _tokenSources.Remove(tokenSource);
                tokenSource.Dispose();

                RemoveFromQueue(id);
            }
        }

        private void RemoveFromQueue(string id)
        {
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                var item = session.Load<QueueItem>(id);
                item.FinishedUtc = DateTime.UtcNow;
                item.QueueStatus = QueueStatus.Finished;

                session.SaveChanges();

                _itemsInExecution.Remove(item.ExecId);
            }
        }

        public async Task EvaluateForExecution()
        {
            await Task.Run(() =>
                {
                    using (var session = RavenDb.DocumentStore.OpenSession())
                    {
                        var environments = session.Query<ConDepEnvConfig>();

                        foreach (var env in environments)
                        {
                            EvaluateForExecution(env.EnvironmentName);
                        }
                    }
                });
        }

        protected void EvaluateForExecution(string environment)
        {
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                RavenQueryStatistics stats;
                var itemsStillInQueue = session.Query<QueueItem>()
                    .Statistics(out stats)
                    .Where(x => x.ExecutionData.Environment == environment && x.QueueStatus != QueueStatus.Finished)
                    .OrderBy(order => order.CreatedUtc)
                    .Customize(x => x.WaitForNonStaleResults());

                var firstItem = itemsStillInQueue.FirstOrDefault();
                if (firstItem == null) return;
                if (_itemsInExecution.ContainsKey(firstItem.ExecId)) return;
                
                _itemsInExecution.Add(firstItem.ExecId, firstItem);
                Execute(string.Format("execution_queue/{0}", firstItem.ExecId));
            }
        }

        public void Cancel()
        {
            foreach (var tokenSource in _tokenSources)
            {
                tokenSource.Cancel();
            }
        }
    }
}
