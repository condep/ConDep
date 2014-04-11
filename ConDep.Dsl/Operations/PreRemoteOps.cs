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
        const string NODE_LISTEN_URL = "http://{0}:80/ConDepNode/";

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            Logger.WithLogSection("Pre-Operations", () =>
                {
                    server.GetServerInfo().TempFolderDos = string.Format(TMP_FOLDER, "%windir%", ConDepGlobals.ExecId);
                    Logger.Info(string.Format("Dos temp folder is {0}", server.GetServerInfo().TempFolderDos));

                    server.GetServerInfo().TempFolderPowerShell = string.Format(TMP_FOLDER, "$env:windir", ConDepGlobals.ExecId);
                    Logger.Info(string.Format("PowerShell temp folder is {0}", server.GetServerInfo().TempFolderDos));

                    TempInstallConDepNode(server, settings);

                    Logger.WithLogSection("Copying PowerShell Scripts", () =>
                        {
                            CopyResourceFiles(Assembly.GetExecutingAssembly(), PowerShellResources.PowerShellScriptResources, server, settings);

                            if (settings.Options.Assembly != null)
                            {
                                var assemblyResources = settings.Options.Assembly.GetManifestResourceNames();
                                CopyResourceFiles(settings.Options.Assembly, assemblyResources, server, settings);
                            }
                        });
                });

        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        private void CopyResourceFiles(Assembly assembly, IEnumerable<string> resources, ServerConfig server, ConDepSettings settings)
        {
            if (resources == null || assembly == null) return;
            
            foreach (var path in resources.Select(resource => ExtractPowerShellFileFromResource(assembly, resource)).Where(path => !string.IsNullOrWhiteSpace(path)))
            {
                CopyFile(path, server, settings);
            }
            var src = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "ConDep.Remote.dll");
            var dst = string.Format(@"{0}\{1}", server.GetServerInfo().TempFolderDos, "ConDep.Remote.dll");
            CopyFile(src, dst, server, settings);
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

        private void CopyFile(string srcPath, ServerConfig server, ConDepSettings settings)
        {
            var dstPath = string.Format(@"{0}\PSScripts\ConDep\{1}", server.GetServerInfo().TempFolderDos, Path.GetFileName(srcPath));
            CopyFile(srcPath, dstPath, server, settings);
        }

        private void CopyFile(string srcPath, string dstPath, ServerConfig server, ConDepSettings settings)
        {
            var filePublisher = new FilePublisher();
            filePublisher.PublishFile(srcPath, dstPath, server, settings);
        }

        private void TempInstallConDepNode(ServerConfig server, ConDepSettings settings)
        {
            Logger.WithLogSection("Deploying ConDep Node", () =>
                {
                    string path;

                    var executionPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConDepNode.exe");
                    if (!File.Exists(executionPath))
                    {
                        var currentPath = Path.Combine(Directory.GetCurrentDirectory(), "ConDepNode.exe");
                        if (!File.Exists(currentPath))
                        {
                            throw new FileNotFoundException("Could not find ConDepNode.exe. Paths tried: \n" +
                                                            executionPath + "\n" + currentPath);
                        }
                        path = currentPath;
                    }
                    else
                    {
                        path = executionPath;
                    }

                    var byteArray = File.ReadAllBytes(path);
                    var nodePublisher = new ConDepNodePublisher(byteArray, Path.Combine(server.GetServerInfo().TempFolderPowerShell, Path.GetFileName(path)), string.Format(NODE_LISTEN_URL, "localhost"), settings);
                    nodePublisher.Execute(server);
                    if (!nodePublisher.ValidateNode(string.Format(NODE_LISTEN_URL, server.Name), server.DeploymentUser.UserName, server.DeploymentUser.Password))
                    {
                        throw new ConDepNodeValidationException("Unable to make contact witstring.Format(listenUrl, server.Name)h ConDep Node or return content from API.");
                    }

                    Logger.Info(string.Format("ConDep Node successfully deployed to {0}", server.Name));
                    Logger.Info(string.Format("Node listening on {0}", string.Format(NODE_LISTEN_URL, server.Name)));
                });
        }

        public string Name { get { return "Pre-Operation"; } }

    }
}