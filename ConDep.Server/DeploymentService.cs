using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Commands;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Execution;
using ConDep.Server.Infrastructure;
using ConDep.Server.Model.DeploymentAggregate;
using Raven.Client;

namespace ConDep.Server
{
    public class DeploymentService : 
        IHandleCommand<Deploy>,
        IHandleCommand<CancelDeployment>
    {
        private readonly Dictionary<Guid, CancellationTokenSource> _tokenSources = new Dictionary<Guid, CancellationTokenSource>();
 
        public DeploymentService(IDocumentSession session)
        {
            Session = session;
        }

        public async Task<IAggregateRoot> Execute(Deploy command)
        {
            var result = new ConDepExecutionResult(false);
            var deployment = Session.Load<Deployment>(command.Id);
            if (deployment == null) throw new ConDepDeploymentNotFound();

            var relativeLogPath = Path.Combine("logs", deployment.Environment, deployment.Id + ".log");
            var tokenSource = new CancellationTokenSource();

            AppDomain appDomain = null;
            try
            {
                appDomain = AppDomain.CreateDomain("ConDepExec", null, AppDomain.CurrentDomain.SetupInformation);

                var executorCancelable = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);
                tokenSource.Token.Register(executorCancelable.Cancel);

                var executor = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);

                var settings = new ConDepSettings
                {
                    Options = new ConDepOptions
                    {
                        Application = deployment.Artifact,
                        Environment = deployment.Environment
                    },
                    Config = Session.Load<ConDepEnvConfig>(RavenDb.GetFullId<ConDepEnvConfig>(deployment.Environment))
                };

                foreach (var server in settings.Config.Servers.Where(server => !server.DeploymentUser.IsDefined()))
                {
                    server.DeploymentUser = settings.Config.DeploymentUser;
                }

                deployment.AddExecutionEvent("Executing now");

                _tokenSources.Add(command.Id, tokenSource);
                executor.Execute(executorCancelable, deployment.Id, relativeLogPath, settings, deployment.Module,
                                     deployment.Artifact, deployment.Environment)
                                     .ContinueWith(task =>
                                         {
                                             result = task.Result;
                                         });

            }
            finally
            {
                if (appDomain != null)
                {
                    try
                    {
                        AppDomain.Unload(appDomain);
                    }
                    catch (Exception ex)
                    {
                        deployment.AddException(ex);
                    }
                }

                try
                {
                    _tokenSources.Remove(deployment.Id);

                    deployment.AddExecutionEvent("Execution finished.");

                    if (result.HasExceptions())
                    {
                        foreach (var ex in result.ExceptionMessages)
                        {
                            deployment.AddException(ex.DateTime, ex.Exception);
                        }
                    }

                    if (result.Success)
                    {
                        deployment.Finish(DeploymentStatus.Success, relativeLogPath);
                    }
                    else if (result.Cancelled)
                    {
                        deployment.Finish(DeploymentStatus.Cancelled, relativeLogPath);
                    }
                    else if (!result.Success)
                    {
                        deployment.Finish(DeploymentStatus.Failed, relativeLogPath);
                    }
                    tokenSource.Dispose();
                }
                catch (Exception ex)
                {
                    deployment.AddException(DateTime.Now, ex);
                }
            }
            return deployment;
        }

        //Todo: Need to handle scoping of token sources for this to work
        public async Task<IAggregateRoot> Execute(CancelDeployment command)
        {
            if (command.Id != Guid.Empty)
            {
                Cancel(command.Id);
            }
            else
            {
                Cancel();
            }
            return null;
        }

        private void Cancel()
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

        private void Cancel(Guid execId)
        {
            if (!_tokenSources.ContainsKey(execId)) return;

            using (var tokenSource = _tokenSources[execId])
            {
                if (tokenSource == null) return;

                tokenSource.Cancel();
                _tokenSources.Remove(execId);
            }
        }

        public IDocumentSession Session { get; private set; }
    }

    public class ConDepDeploymentNotFound : Exception
    {
    }
}
