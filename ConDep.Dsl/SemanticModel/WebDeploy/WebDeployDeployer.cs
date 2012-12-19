using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using ConDep.Dsl.Config;
using Microsoft.Win32;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeployDeployer : IDisposable
    {
        private readonly ServerConfig _server;
        private bool _remoteNeedsCleanup;
        private string _remoteServerGuid;
        private ManagementObject _processObject;
        private bool _disposed = false;
        private string _webDeployInstallPath;

        public WebDeployDeployer(ServerConfig server)
        {
            _server = server;

            try
            {
                Init();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        private void Init()
        {
            CreateRemoteDirectories();
            InstallWebDeployFilesOnServer();
            StartWebDeployServiceOnRemoteServer();
        }

        private void CreateRemoteDirectories()
        {
            var directorySecurity = new DirectorySecurity();
            directorySecurity.SetSecurityDescriptorSddlForm("D:PAI(A;;FA;;;BA)");

            var remoteRootDir = Directory.CreateDirectory(RemotePath, directorySecurity);
            _remoteNeedsCleanup = true;

            var subDirs = new [] { "x86", "x64", "Extensibility" };
            foreach (var str in subDirs)
            {
                remoteRootDir.CreateSubdirectory(str);
            }
        }

        private void InstallWebDeployFilesOnServer()
        {
            var config = @"
<configuration>
    <startup  useLegacyV2RuntimeActivationPolicy='true' >
        <supportedRuntime version='v4.0' />
        <supportedRuntime version='v2.0.50727' />
    </startup>
    <runtime>
        <assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>
            <dependentAssembly>
                <assemblyIdentity name='Microsoft.Web.Deployment' publicKeyToken='31bf3856ad364e35' culture='neutral' />
                <bindingRedirect oldVersion='{0}' newVersion='{1}' />
            </dependentAssembly>
        </assemblyBinding>
    </runtime>
</configuration>";
            config = string.Format(config, "7.1.0.0", "9.0.0.0");

            File.Copy(Path.Combine(WebDeployV1InstallPath, "MsDepSvc.exe"), Path.Combine(RemotePath, "MsDepSvc.exe"));

            var files = new List<string> { "Microsoft.Web.Deployment.dll", "Microsoft.Web.Delegation.dll", @"x64\axnative.dll", @"x86\axnative.dll" };
            files.ForEach(fileName => File.Copy(Path.Combine(WebDeployInstallPath, fileName), Path.Combine(RemotePath, fileName)));

            using (var writer = new StreamWriter(Path.Combine(RemotePath, "MsDepSvc.exe.config")))
            {
                writer.WriteLine(config);
            }
        }

        private static ManagementScope GetManagementScope(ServerConfig server)
        {
            var options = new ConnectionOptions
            {
                Impersonation = ImpersonationLevel.Impersonate,
                EnablePrivileges = true
            };
            if ((server.DeploymentUser != null))
            {
                options.Username = server.DeploymentUser.UserName;
                options.Password = server.DeploymentUser.Password;
            }
            var scope = new ManagementScope(string.Format(CultureInfo.InvariantCulture, @"\\{0}\ROOT\CIMV2", server.Name), options);
            //try
            //{
                scope.Connect();
            //}
            //catch (COMException exception)
            //{
                //if (COMHelper.IsExceptionSameAsError(exception, (ErrorCode)(-2147023174)))
                //{
                //throw new DeploymentFatalException(exception, "CannotConnectToRemoteWMI", new object[] { computerNameWithoutPort });
                //}
            //}
            return scope;
        }

        private void StartWebDeployServiceOnRemoteServer()
        {
            var options = new ObjectGetOptions();

            using (var managementClass = new ManagementClass(GetManagementScope(_server), new ManagementPath("Win32_Process"), options))// GetManagementClass(this.ManagementScope, "Win32_Process"))
            {
                using (ManagementBaseObject managementBaseObject = managementClass.GetMethodParameters("Create"))
                {
                    string remoteProcessId;
                    string port = "80";
                    string listenUrl = "http://+:" + port + "/" + RemoteServerGuid + "/";
                    
                    _server.WebDeployAgentUrl = "http://" + _server.Name + ":" + port + "/" + RemoteServerGuid;
                    
                    string remoteExePath = Path.Combine(RemotePath, "MsDepSvc.exe") + " -listenUrl:" + listenUrl;
                    managementBaseObject["CommandLine"] = remoteExePath;

                    using (ManagementBaseObject management = managementClass.InvokeMethod("Create", managementBaseObject, null))
                    {
                        remoteProcessId = management["ProcessId"].ToString();
                    }

                    //string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSDEPLOYBREAK"));
                    var query = new ObjectQuery("SELECT * FROM Win32_Process WHERE ProcessId = " + remoteProcessId);
                    using (var searcher = new ManagementObjectSearcher(GetManagementScope(_server), query))
                    {
                        using (ManagementObjectCollection objects = searcher.Get())
                        {
                            foreach (ManagementObject managementObject in objects)
                            {
                                _processObject = managementObject;
                            }
                        }
                    }

                }
            }
        }

        protected string AgentUrl { get; set; }

        private string RemotePath
        {
            get
            {
                var remoteTemp = GetRemoteTemp(_server);

                var remotePath = string.Format(@"{0}\MSDEPLOY\{1}", remoteTemp, RemoteServerGuid);
                var uncPath = ConvertToRemotePath(remotePath, _server.Name);
                return uncPath;
            }
        }

        protected string RemoteServerGuid
        {
            get
            {
                if(_remoteServerGuid == null)
                {
                    _remoteServerGuid = Guid.NewGuid().ToString();
                }
                return _remoteServerGuid;
            }
        }

        private bool Is64Bit
        {
            get { return IntPtr.Size == 8; }
        }

        private bool Is32Bit
        {
            get { return IntPtr.Size == 4; }
        }

        private bool Is32BitWOW
        {
            get { return Is32Bit && !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")); }
        }

        private string WebDeployInstallPath
        {
            get
            {
                if(_webDeployInstallPath == null)
                {
                    var installPath = "";
                    var installPathx64 = "";

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\IIS Extensions\\MSDeploy\\3"))
                    {
                        var installPathValue = key.GetValue("InstallPath");
                        installPath = installPathValue == null ? "" : installPathValue.ToString();

                        var installPathx64Value = key.GetValue("InstallPath_x64");
                        installPathx64 = installPathx64Value == null ? "" : installPathx64Value.ToString();
                    }


                    string str = Is64Bit ? (!Is32BitWOW ? installPath : installPathx64) : installPath;
                    _webDeployInstallPath = string.IsNullOrWhiteSpace(str) ? string.Empty : str;
                }
                return _webDeployInstallPath;
            }
        }

        private string WebDeployV1InstallPath
        {
            get { return Path.Combine(new DirectoryInfo(WebDeployInstallPath).Parent.FullName, "Microsoft Web Deploy"); }
        }

        private static string GetRemoteTemp(ServerConfig server)
        {
            var options = new ConnectionOptions
            {
                Impersonation = ImpersonationLevel.Impersonate,
                EnablePrivileges = true
            };
            if (server.DeploymentUser != null)
            {
                options.Username = server.DeploymentUser.UserName;
                options.Password = server.DeploymentUser.Password;
            }

            Exception innerException = null;
            try
            {
                var manScope = new ManagementScope(string.Format("\\\\{0}\\root\\CIMV2", server.Name), options);
                manScope.Connect();

                using (var managementClass = new ManagementClass(manScope, new ManagementPath("Win32_OperatingSystem"), new ObjectGetOptions()))
                {
                    foreach (var item in managementClass.GetInstances())
                    {
                        using (item)
                        {
                            var propertyValue = item.GetPropertyValue("WindowsDirectory") as string;
                            if (!string.IsNullOrEmpty(propertyValue))
                            {
                                return propertyValue + @"\TEMP";
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                innerException = exception2;
            }
            throw new DirectoryNotFoundException(string.Format("Could not find system temp folder (%temp%) on remote server [{0}]", server.Name), innerException);
        }

        public static string ConvertToRemotePath(string localPath, string computerName)
        {
            if (string.IsNullOrEmpty(computerName) || !char.IsLetter(localPath[0]) || (int)localPath[1] != 58)
                return localPath;
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("\\\\");
            stringBuilder.Append(computerName);
            stringBuilder.Append("\\");
            stringBuilder.Append(localPath[0]);
            stringBuilder.Append("$");
            stringBuilder.Append(localPath.Substring(2));
            return stringBuilder.ToString();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            
            if (disposing)
            {
                RemoveWebDeployServiceFromServer();
            }
            _disposed = true;
        }

        ~WebDeployDeployer()
        {
            Dispose(false);
        }

        private void RemoveWebDeployServiceFromServer()
        {
            if (this._processObject != null)
            {
                try
                {
                    _processObject.InvokeMethod("Terminate", new object[] {0});
                }
                catch (ManagementException exception)
                {
                    //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(TraceLevel.Warning, Resources.DeleteElementErrorMessage, new object[] { obj2, "remote process", exception }));
                }
                finally
                {
                    _processObject.Dispose();
                    _processObject = null;
                    GC.SuppressFinalize(this);
                }
            }
            if (_remoteNeedsCleanup)
            {
                //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.DeletingTempAgentDirectory, new object[] { this.RemoteDestShare }));
                int retryAttempt = 0;
                do
                {
                    Exception exception2 = new IOException();
                    try
                    {
                        if (Directory.Exists(RemotePath))
                        {
                            Directory.Delete(RemotePath, true);
                        }
                        _remoteNeedsCleanup = false;
                    }
                    catch (UnauthorizedAccessException exception3)
                    {
                        exception2 = exception3;
                        //Tracer.TraceError(DeploymentTraceSource.Agent, exception3.ToString());
                    }
                    catch (IOException exception4)
                    {
                        exception2 = exception4;
                        //Tracer.TraceError(DeploymentTraceSource.Agent, exception4.ToString());
                    }
                    if (_remoteNeedsCleanup)
                    {
                        if (++retryAttempt > RetryAttempts)
                        {
                            //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(TraceLevel.Warning, Resources.DeleteElementErrorMessage, new object[] { this.RemoteDestShare, Resources.DirPathProviderDescription, exception2 }));
                            _remoteNeedsCleanup = false;
                        }
                        else
                        {
                            //this.BaseContext.RaiseEvent(new DeploymentRetryEventArgs(exception2.Message, DeploymentOperationKind.Delete, "dirPath", this.RemoteDestShare, retryAttempt, this.BaseContext.RetryAttempts));
                            Thread.Sleep(RetryInterval);
                        }
                    }
                } while (_remoteNeedsCleanup);
            }
        }

        protected int RetryInterval
        {
            get { return 5; }
        }

        protected int RetryAttempts
        {
            get { return 3; }
        }
    }
}