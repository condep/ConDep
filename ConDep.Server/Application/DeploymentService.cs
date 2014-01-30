using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Config;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Domain.Deployment;
using ConDep.Server.Domain.Deployment.Model;
using ConDep.Server.Domain.Infrastructure;
using ConDep.Server.Execution;

namespace ConDep.Server.Application
{
    public class DeploymentService 
    {
        private readonly ICommandBus _commandBus;
        private static readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokenSources = new ConcurrentDictionary<Guid, CancellationTokenSource>();
 
        public DeploymentService(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public void Deploy(ExecutionData execData, ConDepEnvConfig config)
        {
            if (execData == null) throw new ConDepDeploymentNotFound();

            Task.Run(() =>
                {
                    var result = new ConDepExecutionResult(false);
                    FileInfo assemblyFile = null;

                    var relativeLogPath = Path.Combine("logs", execData.Environment, execData.DeploymentId + ".log");
                    var tokenSource = new CancellationTokenSource();

                    AppDomain appDomain = null;
                    try
                    {
                        appDomain = AppDomain.CreateDomain("ConDepExec", null, AppDomain.CurrentDomain.SetupInformation);

                        var executorCancelable = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);
                        tokenSource.Token.Register(executorCancelable.Cancel);

                        var executor = (ConDepExecutor)appDomain.CreateInstanceAndUnwrap(typeof(ConDepExecutor).Assembly.FullName, typeof(ConDepExecutor).FullName);

                        var settings = CreateConDepSettings(execData, config);

                        AddExecutionEvent(execData.DeploymentId, "Executing now");

                        _tokenSources.TryAdd(execData.DeploymentId, tokenSource);

                        assemblyFile = CopyAssemblyToTempLocation(execData.DeploymentId, execData.Module);

                        var logPathCmd = new SetDeploymentLogLocation(execData.DeploymentId, relativeLogPath);
                        _commandBus.Send(logPathCmd);

                        result = executor.Execute(executorCancelable, execData.DeploymentId, relativeLogPath, settings, assemblyFile.FullName,
                                                execData.Artifact, execData.Environment);
                    }
                    finally
                    {
                        if (appDomain != null)
                        {
                            try
                            {
                                AppDomain.Unload(appDomain);
                                if (assemblyFile != null)
                                {
                                    assemblyFile.Delete();
                                }
                            }
                            catch (Exception ex)
                            {
                                AddException(execData.DeploymentId, ex);
                            }
                        }

                        try
                        {
                            CancellationTokenSource token;
                            if (_tokenSources.TryRemove(execData.DeploymentId, out token))
                            {
                                token.Dispose();
                            }

                            AddExecutionEvent(execData.DeploymentId, "Execution finished");

                            if (result.HasExceptions())
                            {
                                foreach (var ex in result.ExceptionMessages)
                                {
                                    AddException(execData.DeploymentId, ex);
                                }
                            }

                            FinishDeployment(execData.DeploymentId, result, relativeLogPath);
                        }
                        catch (Exception ex)
                        {
                            AddException(execData.DeploymentId, ex);
                        }
                    }
                });
        }

        private void FinishDeployment(Guid deploymentId, ConDepExecutionResult result, string relativeLogPath)
        {
            if (result.Success)
            {
                var finishDeployment = new FinishDeployment(deploymentId, DeploymentStatus.Success, relativeLogPath);
                _commandBus.Send(finishDeployment);
            }
            else if (result.Cancelled)
            {
                var finishDeployment = new FinishDeployment(deploymentId, DeploymentStatus.Cancelled, relativeLogPath);
                _commandBus.Send(finishDeployment);
            }
            else if (!result.Success)
            {
                var finishDeployment = new FinishDeployment(deploymentId, DeploymentStatus.Failed, relativeLogPath);
                _commandBus.Send(finishDeployment);
            }
        }

        private void AddException(Guid deploymentId, TimedException timedException)
        {
            var exceptionCmd = new AddDeploymentTimedException(deploymentId, timedException);
            _commandBus.Send(exceptionCmd);
        }

        private void AddException(Guid deploymentId, Exception ex)
        {
            var exceptionCmd = new AddDeploymentException(deploymentId, ex);
            _commandBus.Send(exceptionCmd);
        }

        private void AddExecutionEvent(Guid deploymentId, string message)
        {
            var executionEvent = new AddDeploymentExecutionEvent(deploymentId, message);
            _commandBus.Send(executionEvent);
        }

        private static ConDepSettings CreateConDepSettings(ExecutionData execData, ConDepEnvConfig config)
        {
            var settings = new ConDepSettings
                {
                    Options = new ConDepOptions
                        {
                            Application = execData.Artifact,
                            Environment = execData.Environment
                        },
                    Config = config
                };

            foreach (var server in settings.Config.Servers.Where(server => !server.DeploymentUser.IsDefined()))
            {
                server.DeploymentUser = settings.Config.DeploymentUser;
            }
            return settings;
        }

        private FileInfo CopyAssemblyToTempLocation(Guid execId, string module)
        {
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var modulesDir = Path.Combine(executingPath, "modules");

            var fileInfo = new FileInfo(Path.Combine(modulesDir, module + ".dll"));
            var tempDir = Path.Combine(modulesDir, "temp");

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            var newFilePath = Path.Combine(tempDir, execId + ".dll");

            return fileInfo.CopyTo(newFilePath);
        }


        public void Cancel(Guid execId)
        {
            if (!_tokenSources.ContainsKey(execId)) return;

            Task.Run(() =>
                {
                    CancellationTokenSource token;
                    if (_tokenSources.TryGetValue(execId, out token))
                    {
                        token.Cancel();
                        if (_tokenSources.TryRemove(execId, out token))
                        {
                            token.Dispose();
                        }
                    }
                });
        }
    }
}
