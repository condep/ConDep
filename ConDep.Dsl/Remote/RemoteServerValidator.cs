using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Remote
{
    internal class RemoteServerValidator
    {
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly ServerInfoHarvester _serverInfoHarvester;

        public RemoteServerValidator(IEnumerable<ServerConfig> servers, ServerInfoHarvester serverInfoHarvester)
        {
            _servers = servers;
            _serverInfoHarvester = serverInfoHarvester;
        }

        public bool IsValid()
        {
            bool isValid = true;
            try
            {
                foreach (var server in _servers)
                {
                    try
                    {
                        Logger.LogSectionStart(string.Format("Validating {0}", server.Name));
                        if (!ValidateWinRm(server))
                        {
                            return false;
                        }

                        Logger.Info("Collecting server data...");
                        _serverInfoHarvester.Harvest(server);
                        Logger.Info("Server data collected");

                        Logger.Info(string.Format("Validating {0}", server.Name));

                        if (HaveNet40(server))
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

        private static bool ValidateWinRm(ServerConfig server)
        {
            Logger.Info("Validating WinRM");
            Logger.Info(string.Format("Checking for WinRM (PowerShell Remoting) on [{0}]...",
                                                    server.Name));

            if (HaveAccessToServer(server))
            {
                Logger.Info(string.Format("Successfully validated WinRM on [{0}].", server.Name));
            }
            else
            {
                Logger.Error(
                    string.Format("Could not connect to remote server [{0}] with WinRM (PowerShell Remoting).",
                                    server.Name));
                Logger.Error(string.Format("Server requirements on [{0}] are NOT OK.", server.Name));
                return false;
            }
            return true;
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
                                    Arguments =
                                        string.Format("id -r:{0} -u:{1} -p:\"{2}\"", server.Name,
                                                      server.DeploymentUser.UserName, server.DeploymentUser.Password),
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
            Logger.Info(string.Format("Checking if .NET Framework 4.0 is installed on server [{0}]...", server.Name));
            var success = server.ServerInfo.DotNetFrameworks.HasVersion(DotNetVersion.v4_0_full);

            if (success)
            {
                Logger.Info(string.Format("Microsoft .NET Framework version 4.0 is installed on server [{0}].",
                                          server.Name));
            }
            else
            {
                Logger.Error(string.Format("Missing Microsoft .NET Framework version 4.0 on [{0}].", server.Name));
            }
            return success;
        }
    }
}