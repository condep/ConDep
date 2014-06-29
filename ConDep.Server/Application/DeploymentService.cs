using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Server.Domain.Deployment.Commands;
using ConDep.Server.Domain.Deployment.Model;
using ConDep.Server.Domain.Environment.Model;
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

        public void Deploy(ExecutionData execData, DeploymentEnvironment environment)
        {
            if (execData == null) throw new ArgumentNullException("execData");
            if (environment == null) throw new ArgumentNullException("environment");

            Task.Run(() =>
                {
                    var result = new ExecutionResult();
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

                        AddExecutionEvent(execData.DeploymentId, "Executing now");

                        _tokenSources.TryAdd(execData.DeploymentId, tokenSource);

                        assemblyFile = CopyAssemblyToTempLocation(execData.DeploymentId, execData.Module);

                        var logPathCmd = new SetDeploymentLogLocation(execData.DeploymentId, relativeLogPath);
                        _commandBus.Send(logPathCmd);

                        result = executor.Execute(executorCancelable, execData.DeploymentId, relativeLogPath, assemblyFile.FullName,
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
                                foreach (var ex in result.Exceptions)
                                {
                                    AddException(execData.DeploymentId, new TimedException{ DateTime = ex.Item1, Exception = ex.Item2 });
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

        private void FinishDeployment(Guid deploymentId, ExecutionResult result, string relativeLogPath)
        {
            FinishDeployment finishDeployment;
            switch (result.Status)
            {
                case ExecutionStatus.Success:
                    finishDeployment = new FinishDeployment(deploymentId, DeploymentStatus.Success, relativeLogPath);
                    _commandBus.Send(finishDeployment);
                    break;
                case ExecutionStatus.Cancelled:
                    finishDeployment = new FinishDeployment(deploymentId, DeploymentStatus.Cancelled, relativeLogPath);
                    _commandBus.Send(finishDeployment);
                    break;
                case ExecutionStatus.Failure:
                    finishDeployment = new FinishDeployment(deploymentId, DeploymentStatus.Failed, relativeLogPath);
                    _commandBus.Send(finishDeployment);
                    break;
                default:
                    throw new ConDepStatusUnknowException();
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
    }
}
