using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ConDep.Dsl.Config;
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

        public async Task<ConDepExecutionResult> Execute(ITokenSource tokenSource, Guid execId, string relativeLogPath, ConDepSettings settings, string module, string artifact, string env)
        {
            //var mergedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, _cts.Token);
            XmlConfigurator.Configure();

            Logger.Initialize(new FileLogger(relativeLogPath, LogManager.GetLogger("condep-file")));

            try
            {
                settings.Options.Assembly = LoadAssembly(module);

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
                    return result;
                }
                catch (OperationCanceledException)
                {
                    return new ConDepExecutionResult(false) { Cancelled = true};
                }
            }
            catch (AggregateException aggEx)
            {
                var result = new ConDepExecutionResult(false);
                aggEx.Handle(inner =>
                    {
                        if (inner is OperationCanceledException)
                        {
                            result.Cancelled = true;
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
                var result = new ConDepExecutionResult(false);
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

        private Assembly LoadAssembly(string module)
        {
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileInfo = new FileInfo(Path.Combine(executingPath, "modules", module + ".dll"));

            return Assembly.LoadFile(fileInfo.FullName);
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