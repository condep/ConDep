using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
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
            Logger.Info(string.Format("Checking if WMI can be used to reach remote server [{0}]...", server.Name));
            var success = false;
            try
            {
                var registry = new RemoteRegistry(server.Name, server.DeploymentUser.UserName, server.DeploymentUser.Password);
                string windowsName;
                success = registry.TryGetStringValue(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", out windowsName);
                if(success)
                {
                    Logger.Info(string.Format("Contact was made with server [{0}] using WMI. Server is {1}.", server.Name, windowsName));
                }
                else
                {
                    Logger.Error(string.Format("Unable to reach server [{0}] using WMI", server.Name));
                }
            }
            catch(UnauthorizedAccessException accessException)
            {
                Logger.Error(string.Format("Unable to access remote server [{0}] using WMI. Unauthorized Access Exception reported. Please check your credentials.", server.Name), accessException);
                return false;
            }
            catch(Exception ex)
            {
                Logger.Error(string.Format("Unable to access remote server [{0}] using WMI.", server.Name), ex);
                return false;
            }
            return success;
        }

        private static bool HaveNet40(ServerConfig server)
        {
            try
            {
                Logger.Info(string.Format("Checking if WMI can be used to check if .NET Framework 4.0 is installed on server [{0}]...", server.Name));
                var registry = new RemoteRegistry(server.Name, server.DeploymentUser.UserName, server.DeploymentUser.Password);

                int dotNet40Installed;
                var success = registry.TryGetDWordValue(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", "Install", out dotNet40Installed);
                if (!success || dotNet40Installed != 1)
                {
                    Logger.Error(string.Format("Missing Microsoft .NET Framework version 4.0 on [{0}].", server.Name));
                    return false;
                }
                
                Logger.Error(string.Format("Microsoft .NET Framework version 4.0 is installed on server [{0}].", server.Name));
                return true;
            }
            catch(Exception ex)
            {
                Logger.Error(string.Format("Unable to access remote server to check for .NET Framework 4.0 on server [{0}] using WMI.", server.Name), ex);
                return false;
            }
        }
    }
}