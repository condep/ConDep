using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.PSScripts;
using ConDep.Dsl.Remote;
using ConDep.Dsl.Resources;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations
{
    internal class PreRemoteOps : IOperateRemote
    {
        const string TMP_FOLDER = @"{0}\temp\ConDep\{1}";

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            try
            {
                Logger.LogSectionStart("Pre-Operations");

                server.GetServerInfo().TempFolderDos = string.Format(TMP_FOLDER, "%windir%", ConDepGlobals.ExecId);
                Logger.Info(string.Format("Dos temp folder is {0}", server.GetServerInfo().TempFolderDos));

                server.GetServerInfo().TempFolderPowerShell = string.Format(TMP_FOLDER, "$env:windir", ConDepGlobals.ExecId);
                Logger.Info(string.Format("PowerShell temp folder is {0}", server.GetServerInfo().TempFolderDos));

                TempInstallConDepNode(status, server);

                try
                {
                    Logger.LogSectionStart("Copying PowerShell Scripts");
                    CopyResourceFiles(Assembly.GetExecutingAssembly(), PowerShellResources.PowerShellScriptResources,
                                      server);

                    if (settings.Options.Assembly != null)
                    {
                        var assemblyResources = settings.Options.Assembly.GetManifestResourceNames();
                        CopyResourceFiles(settings.Options.Assembly, assemblyResources, server);
                    }
                }
                finally
                {
                    Logger.LogSectionEnd("Copying PowerShell Scripts");
                }
            }
            finally
            {
                Logger.LogSectionEnd("Pre-Operations");
            }

        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        private void CopyResourceFiles(Assembly assembly, IEnumerable<string> resources, ServerConfig server)
        {
            if (resources == null || assembly == null) return;
            
            foreach (var path in resources.Select(resource => ExtractPowerShellFileFromResource(assembly, resource)).Where(path => !string.IsNullOrWhiteSpace(path)))
            {
                CopyFile(path, server);
            }
            var src = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ConDep.Remote.dll");
            var dst = string.Format(@"{0}\{1}", server.GetServerInfo().TempFolderDos, "ConDep.Remote.dll");
            CopyFile(src, dst, server);
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

        private void CopyFile(string srcPath, ServerConfig server)
        {
            var dstPath = string.Format(@"{0}\PSScripts\ConDep\{1}", server.GetServerInfo().TempFolderDos, Path.GetFileName(srcPath));
            CopyFile(srcPath, dstPath, server);
        }

        private void CopyFile(string srcPath, string dstPath, ServerConfig server)
        {
            var filePublisher = new FilePublisher();
            filePublisher.PublishFile(srcPath, dstPath, server);
        }

        private void TempInstallConDepNode(IReportStatus status, ServerConfig server)
        {
            Logger.LogSectionStart("Deploying ConDep Node");
            try
            {
                var listenUrl = "http://{0}:80/ConDepNode/";
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConDepNode.exe");
                var byteArray = File.ReadAllBytes(path);
                var nodePublisher = new ConDepNodePublisher(byteArray, Path.Combine(server.GetServerInfo().TempFolderPowerShell, Path.GetFileName(path)), string.Format(listenUrl, "localhost"));
                nodePublisher.Execute(server);
                if (!nodePublisher.ValidateNode(string.Format(listenUrl, server.Name), server.DeploymentUser.UserName, server.DeploymentUser.Password))
                {
                    throw new ConDepNodeValidationException("Unable to make contact witstring.Format(listenUrl, server.Name)h ConDep Node or return content from API.");
                }

                Logger.Info(string.Format("ConDep Node successfully deployed to {0}", server.Name));
                Logger.Info(string.Format("Node listening on {0}", string.Format(listenUrl, server.Name)));
            }
            finally
            {
                Logger.LogSectionEnd("Deploying ConDep Node");
            }
        }
    }
}