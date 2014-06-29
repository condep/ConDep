using System;
using System.Reflection;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Execution;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using log4net;
using log4net.Config;

namespace ConDep.Server.Execution
{
    public class ConDepExecutor : MarshalByRefObject, ITokenSource, IDisposable
    {
        private readonly CancellationTokenSource _cts;

        public ConDepExecutor()
        {
            _cts = new CancellationTokenSource();
        }

        public ExecutionResult Execute(ITokenSource tokenSource, Guid execId, string relativeLogPath, string assemblyFilePath, string artifact, string env)
        {
            //var mergedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, _cts.Token);
            XmlConfigurator.Configure();

            Logger.Initialize(new FileLogger(relativeLogPath, LogManager.GetLogger("condep-file")));

            //ToDo: Handle config settings
            var settings = CreateConDepSettings(env, artifact, new ConDepEnvConfig());

            try
            {
                settings.Options.Assembly = Assembly.LoadFile(assemblyFilePath);

                ConDepGlobals.Reset();
                try
                {
                    var result = ConDepConfigurationExecutor.ExecuteFromAssembly(settings, tokenSource.Token);
                    
                    if (result.Success)
                    {
                        Logger.Info("ConDep finished execution run successfully");
                    }
                    else
                    {
                        Logger.Error("ConDep finished execution run un-successfully");
                    }
                    return CreateResult(result);
                }
                catch (OperationCanceledException)
                {
                    return new ExecutionResult {Status = ExecutionStatus.Cancelled};
                }
            }
            catch (AggregateException aggEx)
            {
                var result = new ExecutionResult();
                aggEx.Handle(inner =>
                    {
                        if (inner is OperationCanceledException)
                        {
                            result.Status = ExecutionStatus.Cancelled;
                            Logger.Warn("ConDep execution cancelled.");
                        }
                        else
                        {
                            result.AddException(inner);
                            Logger.Error("Unhandled exception during deployment", inner);
                        }

                        return true;
                    });

                Logger.Error("ConDep finished execution run with errors");
                return result;
            }
            catch (Exception ex)
            {
                var result = new ExecutionResult();
                try
                {
                    result.AddException(ex);
                    Logger.Error("Unhandled exception during deployment", ex);
                    Logger.Error("ConDep finished execution run with errors");
                    return result;
                }
                catch (Exception innerEx)
                {
                    result.AddException(innerEx);
                    return result;
                }
            }
        }

        private static ConDepSettings CreateConDepSettings(string env, string artifact, ConDepEnvConfig config)
        {
            var settings = new ConDepSettings
            {
                Options = new ConDepOptions
                {
                    Application = artifact,
                    Environment = env
                },
                Config = config
            };

            //foreach (var server in settings.Config.Servers.Where(server => !server.DeploymentUser.IsDefined()))
            //{
            //    server.DeploymentUser = settings.Config.DeploymentUser;
            //}
            return settings;
        }

        private ExecutionResult CreateResult(ConDepExecutionResult result)
        {
            var execResult = new ExecutionResult();
            if (result.Success)
            {
                execResult.Status = ExecutionStatus.Success;
            }
            else if (result.Cancelled)
            {
                execResult.Status = ExecutionStatus.Cancelled;
            }
            else if (!result.Success)
            {
                execResult.Status = ExecutionStatus.Failure;
            }

            if (result.HasExceptions())
            {
                foreach (var ex in result.ExceptionMessages)
                {
                    execResult.AddException(ex.DateTime, ex.Exception);
                }
            }

            return execResult;
        }

        public CancellationToken Token { get { return _cts.Token; } }

        public void Dispose()
        {
            _cts.Dispose();
        }

        public void Cancel()
        {
            Logger.Warn("Cancellation of execution was triggered. Cancelling all operations now...");
            if (_cts != null)
            {
                _cts.Cancel();
            }
            else
            {
                Logger.Warn("CancellationToken was null :-(");
            }
        }
    }
}