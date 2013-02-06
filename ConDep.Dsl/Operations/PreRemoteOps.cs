using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Deployment.CopyFile;
using ConDep.Dsl.PSScripts;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Operations
{
    public class PreRemoteOps
    {
        private readonly ServerConfig _server;
        private readonly PreOpsSequence _sequence;
        private readonly ConDepOptions _options;
        private IHandleWebDeploy _webDeploy;

        public PreRemoteOps(ServerConfig server, PreOpsSequence sequence, ConDepOptions options, IHandleWebDeploy webDeploy)
        {
            _server = server;
            _sequence = sequence;
            _options = options;
            _webDeploy = webDeploy;
        }

        public void Configure()
        {
            ConfigureCopyResource(PowerShellResources.PowerShellScriptResources);

            if(_options.Assembly != null)
            {
                var assemblyResources = _options.Assembly.GetManifestResourceNames().Select(assResource => Resources.ConDepResourceFiles.GetFilePath(_options.Assembly, assResource, true));
                ConfigureCopyResource(assemblyResources);
            }
        }

        private void ConfigureCopyResource(IEnumerable<string> resources)
        {
            foreach (var resource in resources)
            {
                var path = Resources.ConDepResourceFiles.GetFilePath(resource, true);
                var copyOp =
                    new RemoteWebDeployOperation(
                        new CopyFileProvider(path,
                                             string.Format(@"%temp%\ConDep\{0}\PSScripts\ConDep\{1}", ConDepGlobals.ExecId,
                                                           Path.GetFileName(path))), _webDeploy);
                _sequence.Add(copyOp, true);
            }
        }

        public IReportStatus Execute(IReportStatus status)
        {
            TempInstallWebDeploy(status);
            return status;
        }

        private void TempInstallWebDeploy(IReportStatus status)
        {
            if(!_options.WebDeployExist)
            {
                WebDeployDeployer.DeployTo(_server);
            }
        }
    }
}