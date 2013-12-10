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
        private readonly Dictionary<string, CancellationTokenSource> _tokenSources = new Dictionary<string, CancellationTokenSource>();
 
        public QueueExecutor(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        protected void Execute(string id)
        {
            bool success = false;
            QueueItem item = null;
            var tokenSource = new CancellationTokenSource();

            _eventLog.WriteEntry(string.Format("A new execution was added to the queue with id {0}.", id), EventLogEntryType.Information);
            AppDomain appDomain = null;
            try
            {
                appDomain = AppDomain.CreateDomain("ConDepExec", null, AppDomain.CurrentDomain.SetupInformation);

                var executorCancelable = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);
                tokenSource.Token.Register(executorCancelable.Cancel);

                var executor = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);

                ConDepSettings settings;

                using (var session = RavenDb.DocumentStore.OpenSession())
                {
                    item = session.Load<QueueItem>(RavenDb.GetFullId<QueueItem>(id));
                    item.QueueStatus = QueueStatus.InProgress;

                    settings = new ConDepSettings
                    {
                        Options = new ConDepOptions
                        {
                            Application = item.ExecutionData.Artifact,
                            Environment = item.ExecutionData.Environment
                        },
                        Config = session.Load<ConDepEnvConfig>(RavenDb.GetFullId<ConDepEnvConfig>(item.ExecutionData.Environment))
                    };

                    foreach (var server in settings.Config.Servers.Where(server => !server.DeploymentUser.IsDefined()))
                    {
                        server.DeploymentUser = settings.Config.DeploymentUser;
                    }

                    var executionStatus = session.Load<ExecutionStatus>(RavenDb.GetFullId<ExecutionStatus>(id));
                    executionStatus.Events.Add(new ExecutionEvent { DateUtc = DateTime.UtcNow, Message = "Executing now."});

                    session.SaveChanges();
                }

                _tokenSources.Add(id, tokenSource);
                success = executor.Execute(executorCancelable, item.ExecId, settings, item.ExecutionData.Module, item.ExecutionData.Artifact, item.ExecutionData.Environment);
            }
            finally
            {
                if (appDomain != null)
                    AppDomain.Unload(appDomain);

                if(item != null) _tokenSources.Remove(item.ExecId);
                tokenSource.Dispose();

                RemoveFromQueue(id, success);
            }
        }

        private void RemoveFromQueue(string id, bool successfullyExecuted)
        {
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                var item = session.Load<QueueItem>(RavenDb.GetFullId<QueueItem>(id));
                item.FinishedUtc = DateTime.UtcNow;
                item.QueueStatus = QueueStatus.Finished;

                var executionStatus = session.Load<ExecutionStatus>(RavenDb.GetFullId<ExecutionStatus>(id));
                executionStatus.FinishedUtc = DateTime.UtcNow;
                executionStatus.FinishedStatus = successfullyExecuted ? FinishedStatus.Success : FinishedStatus.Failed;

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
            IEnumerable<QueueItem> itemsStillInQueue;
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                var query = new ListItemsStillInQueueQuery(session);
                itemsStillInQueue = query.Execute(environment);
            }

            var firstItem = itemsStillInQueue.FirstOrDefault();
            if (firstItem == null) return;
            if (_itemsInExecution.ContainsKey(firstItem.ExecId)) return;

            _itemsInExecution.Add(firstItem.ExecId, firstItem);
            Execute(firstItem.ExecId);
        }

        public void Cancel()
        {
            foreach (var tokenSource in _tokenSources.Values)
            {
                using (tokenSource)
                {
                    tokenSource.Cancel();
                }
            }
            _tokenSources.Clear();
        }

        public void Cancel(string execId)
        {
            using (var tokenSource = _tokenSources[execId])
            {
                if (tokenSource == null) return;
                
                tokenSource.Cancel();
                _tokenSources.Remove(execId);
            }
        }
    }

    public class ListItemsStillInQueueQuery
    {
        private readonly IDocumentSession _session;

        public ListItemsStillInQueueQuery(IDocumentSession session)
        {
            _session = session;
        }

        public List<QueueItem> Execute(string environment)
        {
            RavenQueryStatistics stats;
            return _session.Query<QueueItem>()
                .Statistics(out stats)
                .Where(x => x.ExecutionData.Environment == environment && x.QueueStatus != QueueStatus.Finished)
                .OrderBy(order => order.CreatedUtc)
                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite()).ToList();

        } 
    }
}
