using System;
using System.IO;
using System.Reflection;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
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

        public bool Execute(ITokenSource tokenSource, string execId, ConDepSettings settings, string module, string artifact, string env)
        {
            bool success;
            XmlConfigurator.Configure();

            Logger.Initialize(new FileLogger(Path.Combine("logs", env), execId, LogManager.GetLogger("condep-file")));

            try
            {
                settings.Options.Assembly = LoadAssembly(module);

                ConDepGlobals.Reset();
                IReportStatus status = new ConDepStatus();
                ConDepConfigurationExecutor.ExecuteFromAssembly(settings, status, tokenSource.Token).Wait();
                Logger.Info("ConDep finished execution run successfully");
                success = true;
            }
            catch (AggregateException aggEx)
            {
                var flattenEx = aggEx.Flatten();
                foreach (var ex in flattenEx.InnerExceptions)
                {
                    Logger.Error("Unhandled exception during deployment", ex);
                }
                Logger.Error("ConDep finished execution run with errors");
                success = false;
            }
            catch (Exception ex)
            {
                try
                {
                    Logger.Error("Unhandled exception during deployment", ex);
                    Logger.Error("ConDep finished execution run with errors");
                }
                catch
                {
                }
                success = false;
            }
            return success;
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
        }
    }
}