using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Raven.Client;

namespace ConDep.Server.Api.Controllers
{
    public class ConDepExecutor
    {
        private ConDepSettings _settings;
        private readonly IDocumentStore _store;
        private readonly string _execId;
        private readonly string _module;
        private readonly string _artifact;
        private readonly string _env;

        public ConDepExecutor(ILogForConDep logger, ConDepSettings settings)
        {
            _settings = settings;
            Logger.Initialize(logger);
        }

        public ConDepExecutor(IDocumentStore store, string execId, string module, string artifact, string env)
        {
            _store = store;
            _execId = execId;
            _module = module;
            _artifact = artifact;
            _env = env;
        }

        public void Execute()
        {
            Logger.Initialize(new ConDepServerLogger(_execId));

            try
            {
                _settings = new ConDepSettings
                    {
                        Options = new ConDepOptions
                            {
                                Assembly = LoadAssembly(_module),
                                Application = _artifact,
                                Environment = _env
                            }
                    };

                LoadEnvConfig(_settings);

                ConDepGlobals.Reset();
                IReportStatus status = new ConDepStatus();
                var success = ConDepConfigurationExecutor.ExecuteFromAssembly(_settings, status);
            }
            catch (Exception ex)
            {
                try
                {
                    Logger.Error("Unhandled exception during deployment", ex);
                }
                catch {}
            }
        }

        private void LoadEnvConfig(ConDepSettings settings)
        {
            using (var session = _store.OpenSession())
            {
                settings.Config = session.Load<ConDepEnvConfig>("environments/" + _env);

                foreach (var server in settings.Config.Servers.Where(server => !server.DeploymentUser.IsDefined()))
                {
                    server.DeploymentUser = settings.Config.DeploymentUser;
                }
            }
        }

        private Assembly LoadAssembly(string module)
        {
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileInfo = new FileInfo(Path.Combine(executingPath, "modules", module + ".dll"));

            return Assembly.LoadFile(fileInfo.FullName);
        }
    }
}