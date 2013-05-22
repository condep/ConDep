using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Remote.Registry;
using Microsoft.Win32;

namespace ConDep.Dsl.Remote
{
    internal class RemoteServerValidator
    {
        private readonly IEnumerable<ServerConfig> _servers;

        public RemoteServerValidator(IEnumerable<ServerConfig> servers)
        {
            _servers = servers;
        }

        public bool IsValid()
        {
            bool isValid = true;
            try
            {
                Logger.LogSectionStart("Validating Servers");
                foreach (var server in _servers)
                {
                    try
                    {
                        Logger.LogSectionStart(string.Format("Validating {0}", server.Name));

                        if (HaveAccessToServer(server) && HaveNet40(server))
                        {
                            Logger.Info(string.Format("Server requirements on [{0}] are OK.", server.Name));
                        }
                        else
                        {
                            Logger.Error(string.Format("Server requirements on [{0}] are NOT OK.", server.Name));
                            isValid = false;
                        }
                    }
                    finally
                    {
                        Logger.LogSectionEnd(string.Format("Validating {0}", server.Name));
                    }
                }
                return isValid;
            }
            finally
            {
                Logger.LogSectionEnd("Validating Servers");
            }
        }

        private static bool HaveAccessToServer(ServerConfig server)
        {
            Logger.Info(
                string.Format("Checking if WinRM (Remote PowerShell) can be used to reach remote server [{0}]...",
                                server.Name));
            var success = false;
            var path = Environment.ExpandEnvironmentVariables(@"%windir%\system32\WinRM.cmd");
            var startInfo = new ProcessStartInfo(path)
                                {
                                    Arguments = string.Format("id -r:{0} -u:{1} -p:\"{2}\"", server.Name, server.DeploymentUser.UserName, server.DeploymentUser.Password),
                                    Verb = "RunAs",
                                    UseShellExecute = false,
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    RedirectStandardError = true,
                                    RedirectStandardOutput = true
                                };
            var process = Process.Start(startInfo);
            process.WaitForExit(10000);

            if (process.ExitCode == 0)
            {
                var message = process.StandardOutput.ReadToEnd();
                Logger.Info(string.Format("Contact was made with server [{0}] using WinRM (Remote PowerShell). ",
                                            server.Name));
                Logger.Verbose(string.Format("Details: {0} ", message));
                success = true;
            }
            else
            {
                var errorMessage = process.StandardError.ReadToEnd();
                Logger.Error(string.Format("Unable to reach server [{0}] using WinRM (Remote PowerShell)",
                                            server.Name));
                Logger.Error(string.Format("Details: {0}", errorMessage));
            }
            return success;
        }

        private static bool HaveNet40(ServerConfig server)
        {
            try
            {
                Logger.Info(string.Format("Checking if .NET Framework 4.0 is installed on server [{0}]...", server.Name));
                var cmd = @"Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full'";// | Get-ItemProperty -name Version -EA 0 | Where { $_.PSChildName -match '^(?!S)\p{L}'} | Select PSChildName, Version";
                var psExecutor = new PowerShellExecutor();
                var result = psExecutor.Execute(server, cmd, logOutput: false, loadConDepModule: false);

                if(result.Count() == 1)//Any(x => x.Version == "4.0.30319" && x.PSChildName == "Full"))
                {
                    Logger.Info(string.Format("Microsoft .NET Framework version 4.0 is installed on server [{0}].", server.Name));
                    return true;
                }

                Logger.Error(string.Format("Missing Microsoft .NET Framework version 4.0 on [{0}].", server.Name));
                return false;
            }
            catch(Exception ex)
            {
                Logger.Error(string.Format("Unable to access remote server to check for .NET Framework 4.0 on server [{0}] using WinRM.", server.Name), ex);
                return false;
            }
        }
    }
}