using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Remote.Registry;
using Microsoft.Win32;

namespace ConDep.Dsl.Impersonation
{
    public class RemoteServerValidator
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
            var registry = new RemoteRegistry(server.Name, server.DeploymentUser.UserName, server.DeploymentUser.Password);

            try
            {
                string windowsName;
                var success = registry.TryGetStringValue(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", out windowsName);
                Logger.Info(string.Format("Server [{0}] are running {1}.", server.Name, windowsName));
            }
            catch(System.UnauthorizedAccessException accessException)
            {
                Logger.Error(string.Format("Unable to access remote server [{0}]. Unauthorized Access Exception reported. Please check your credentials.", server.Name));
                return false;
            }
            return true;
        }

        private static bool HaveNet40(ServerConfig server)
        {
            var registry = new RemoteRegistry(server.Name, server.DeploymentUser.UserName, server.DeploymentUser.Password);

            int dotNet40Installed;
            var success = registry.TryGetDWordValue(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", "Install", out dotNet40Installed);
            if (!success || dotNet40Installed != 1)
            {
                Logger.Error(string.Format("Missing Microsoft .NET Framework version 4.0 on [{0}].", server.Name));
                return false;
            }
            return dotNet40Installed == 1;
        }
    }
}