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

        public void Execute(ITokenSource tokenSource, string execId, ConDepSettings settings, string module, string artifact, string env)
        {
            XmlConfigurator.Configure();

            Logger.Initialize(new FileLogger(Path.Combine("logs", env), execId, LogManager.GetLogger("condep-file")));

            try
            {
                settings.Options.Assembly = LoadAssembly(module);

                ConDepGlobals.Reset();
                IReportStatus status = new ConDepStatus();

                ConDepConfigurationExecutor.ExecuteFromAssembly(settings, status, tokenSource.Token).Wait();
                Logger.Info("ConDep finished execution run successfully");
            }
            catch (Exception ex)
            {
                try
                {
                    Logger.Error("Unhandled exception during deployment", ex);
                    Logger.Error("ConDep finished execution run with errors");
                }
                catch {}
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
            if (_cts != null)
            {
                _cts.Cancel();
            }
        }
    }
}