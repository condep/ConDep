using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Deployment.CopyFile;
using ConDep.Dsl.PSScripts;
using ConDep.Dsl.Resources;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Operations
{
    internal class PreRemoteOps
    {
        private readonly ServerConfig _server;
        private readonly PreOpsSequence _sequence;
        private readonly ConDepSettings _settings;
        private IHandleWebDeploy _webDeploy;

        public PreRemoteOps(ServerConfig server, PreOpsSequence sequence, ConDepSettings settings, IHandleWebDeploy webDeploy)
        {
            _server = server;
            _sequence = sequence;
            _settings = settings;
            _webDeploy = webDeploy;
        }

        public void Configure()
        {
            ConfigureCopyResource(Assembly.GetExecutingAssembly(), PowerShellResources.PowerShellScriptResources);

            if(_settings.Options.Assembly != null)
            {
                var assemblyResources = _settings.Options.Assembly.GetManifestResourceNames();
                ConfigureCopyResource(_settings.Options.Assembly, assemblyResources);
            }
        }

        private void ConfigureCopyResource(Assembly assembly, IEnumerable<string> resources)
        {
            if (resources == null || assembly == null) return;
            
            foreach (var path in resources.Select(resource => ExtractPowerShellFileFromResource(assembly, resource)).Where(path => !string.IsNullOrWhiteSpace(path)))
            {
                ConfigureCopyFileOperation(path);
            }
        }

        private string ExtractPowerShellFileFromResource(Assembly assembly, string resource)
        {
            var regex = new Regex(@".+\.(.+\.(ps1|psm1))");
            var match = regex.Match(resource);
            if (match.Success)
            {
                var resourceName = match.Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(resourceName))
                {
                    var resourceNamespace = resource.Replace("." + resourceName, "");
                    return ConDepResourceFiles.GetFilePath(assembly, resourceNamespace, resourceName, true);
                }
            }
            return null;
        }

        private void ConfigureCopyFileOperation(string path)
        {
            var copyFileProvider = new CopyFileProvider(path, string.Format(@"%temp%\ConDep\{0}\PSScripts\ConDep\{1}", ConDepGlobals.ExecId, Path.GetFileName(path)));
            var copyOp = new RemoteWebDeployOperation(copyFileProvider, _webDeploy);
            _sequence.Add(copyOp, true);
        }

        public IReportStatus Execute(IReportStatus status)
        {
            TempInstallWebDeploy(status);
            return status;
        }

        private void TempInstallWebDeploy(IReportStatus status)
        {
            if(!_settings.Options.WebDeployExist)
            {
                Logger.LogSectionStart("Deploying Web Deploy");
                try
                {
                    WebDeployDeployer.DeployTo(_server);
                }
                finally
                {
                    Logger.LogSectionEnd("Deploying Web Deploy");
                }
            }
        }
    }
}