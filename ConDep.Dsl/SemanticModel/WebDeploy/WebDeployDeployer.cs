using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using Microsoft.Web.Deployment;
using Microsoft.Win32;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public class WebDeployDeployer : IDisposable
    {
        private readonly ServerConfig _server;
        private bool _remoteNeedsCleanup;
        private string _remoteServerGuid;
        private ManagementObject _processObject;
        private bool _disposed;
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
            Logger.Info(string.Format("Deploying Microsoft WebDeploy 2.0 to remote server [{0}]. Make sure you comply with the Web Deploy license on that server.", _server.Name));
            using (new Impersonation.Impersonator(_server.DeploymentUserRemote.UserName, _server.DeploymentUserRemote.Password))
            {
                CreateRemoteDirectories();
                InstallWebDeployFilesOnRemoteServer();
                StartWebDeployServiceOnRemoteServer();
                MakeSureWebDeployEndpointIsRunning();
            }
        }

        private void CreateRemoteDirectories()
        {
            var directorySecurity = new DirectorySecurity();
            directorySecurity.SetSecurityDescriptorSddlForm("D:PAI(A;;FA;;;BA)");

            var remoteRootDir = Directory.CreateDirectory(RemotePath, directorySecurity);
            _remoteNeedsCleanup = true;

            var subDirs = new[] { "x86", "x64", "Extensibility" };
            foreach (var str in subDirs)
            {
                remoteRootDir.CreateSubdirectory(str);
            }
        }

        private void InstallWebDeployFilesOnRemoteServer()
        {
            var config = @"
<configuration>
    <!--<startup>
        <supportedRuntime version='v4.0' />
    </startup>-->
    <runtime>
        <assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>
            <dependentAssembly>
                <assemblyIdentity name='Microsoft.Web.Deployment' publicKeyToken='31bf3856ad364e35' culture='neutral' />
                <bindingRedirect oldVersion='{0}' newVersion='{1}' />
            </dependentAssembly>
        </assemblyBinding>
    </runtime>
</configuration>";
            //WebDeploy 3.0
            //config = string.Format(config, "7.1.0.0", "9.0.0.0");
            
            config = string.Format(config, "7.1.0.0", "7.1.0.0");

            File.Copy(Path.Combine(WebDeployV1InstallPath, "MsDepSvc.exe"), Path.Combine(RemotePath, "MsDepSvc.exe"));

            var files = new List<string> { "Microsoft.Web.Deployment.dll", "Microsoft.Web.Delegation.dll", @"x64\axnative.dll", @"x86\axnative.dll" };
            files.ForEach(fileName => File.Copy(Path.Combine(WebDeployInstallPath, fileName), Path.Combine(RemotePath, fileName)));

            using (var writer = new StreamWriter(Path.Combine(RemotePath, "MsDepSvc.exe.config")))
            {
                writer.WriteLine(config);
            }
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

                    var query = new ObjectQuery("SELECT * FROM Win32_Process WHERE ProcessId = " + remoteProcessId);

                    bool processStarted = false;
                    using (var searcher = new ManagementObjectSearcher(GetManagementScope(_server), query))
                    {
                        using (ManagementObjectCollection objects = searcher.Get())
                        {
                            foreach (ManagementObject managementObject in objects)
                            {
                                _processObject = managementObject;
                                processStarted = true;
                            }
                        }
                    }

                    if(!processStarted)
                    {
                        throw new ConDepWebDeployProviderException("Unable to start Microsoft Web Deploy on remote server. Make sure .NET Framework 4.0 is installed.");
                    }

                }
            }
        }

        private void MakeSureWebDeployEndpointIsRunning()
        {
            try
            {
                var num = 0;
                while (true)
                {
                    try
                    {
                        using (GetHttpResponse()) { }
                        break;
                    }
                    catch
                    {
                        if (++num <= RetryAttempts)
                        {
                            Thread.Sleep(RetryInterval*1000);
                        }
                        else
                            throw;
                    }
                }
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        private static string GetResponseHeader(HttpWebResponse response, string headerName)
        {
            return response == null ? string.Empty : response.Headers[headerName];
        }

        private HttpWebRequest CreateHttpRequest()
        {
            HttpWebRequest request;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(_server.WebDeployAgentUrl);
            }
            catch (UriFormatException ex)
            {
                throw new DeploymentException(ex, "BadURI", new object[] { _server.WebDeployAgentUrl });
            }
            request.Proxy = null;
            request.Credentials = new NetworkCredential(_server.DeploymentUserRemote.UserNameWithoutDomain, _server.DeploymentUserRemote.Password, _server.DeploymentUserRemote.Domain);
            request.UnsafeAuthenticatedConnectionSharing = true;
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = false;
            request.Method = "HEAD";
            request.Headers.Add("MSDeploy.VersionMin", "7.1.600.0");
            request.Headers.Add("MSDeploy.VersionMax", "9.0.1631.0");
            string name1 = CultureInfo.CurrentUICulture.Name;
            string name2 = CultureInfo.CurrentCulture.Name;
            request.Headers.Add("MSDeploy.RequestUICulture", name1);
            request.Headers.Add("MSDeploy.RequestCulture", name2);
            request.Headers.Add("Version", "7.1.0.0");

            //WebDeploy 3.0
            //request.Headers.Add("Version", "9.0.0.0");            
            return request;
        }

        private HttpWebResponse GetHttpResponse()
        {
            var request = CreateHttpRequest();

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;

                using (WebResponse response2 = ex.Response)
                {
                    var response3 = response2 as HttpWebResponse;
                    string responseHeader1 = GetResponseHeader(response3, "MSDeploy.ExceptionMessage");
                    if (!string.IsNullOrEmpty(responseHeader1))
                    {
                        throw new DeploymentException(new DeploymentException(Encoding.UTF8.GetString(Convert.FromBase64String(responseHeader1))), "RemoteDeploymentException", new object[1] { (object) DateTime.Now });
                    }
                    else
                    {
                        var responseHeader2 = GetResponseHeader(response3, "MSDeploy.Exception");
                        if (string.IsNullOrEmpty(responseHeader2))
                            throw;

                        DateTime dateTime = DateTime.Now;
                        string responseHeader3 = GetResponseHeader(response3, "MSDeploy.ExceptionTimeStamp");
                        long result;
                        if (!string.IsNullOrEmpty(responseHeader3) && long.TryParse(responseHeader3, NumberStyles.None, (IFormatProvider)CultureInfo.InvariantCulture, out result))
                            dateTime = DateTime.FromBinary(result).ToLocalTime();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DeploymentException(ex, "AgentRequestFailure", new object[1] { (object) request.Address });
            }
            if (string.Equals(GetResponseHeader(response, "MSDeploy.Response"), "v1", StringComparison.Ordinal))
                return response;
            response.Close();
            throw new DeploymentDetailedException(DeploymentErrorCode.ERROR_DESTINATION_NOT_REACHABLE, "ERROR_DESTINATION_NOT_REACHABLE", new [] { (object) request.Address.Host });
        }

        private static ManagementScope GetManagementScope(ServerConfig server)
        {
            var options = GetWmiConnectionOptions(server);
            var scope = new ManagementScope(string.Format(CultureInfo.InvariantCulture, @"\\{0}\ROOT\CIMV2", server.Name), options);
            scope.Connect();
            return scope;
        }

        private static ConnectionOptions GetWmiConnectionOptions(ServerConfig server)
        {
            var options = new ConnectionOptions
                              {
                                  Impersonation = ImpersonationLevel.Impersonate,
                                  EnablePrivileges = true
                              };
            if ((server.DeploymentUserRemote != null))
            {
                options.Username = server.DeploymentUserRemote.UserName;
                options.Password = server.DeploymentUserRemote.Password;
            }
            return options;
        }

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

        private string WebDeployInstallPath
        {
            get
            {
                if(_webDeployInstallPath == null)
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\IIS Extensions\\MSDeploy\\2"))
                    {
                        if(key == null)
                            throw new ConDepWebDeployProviderException("Could not find Web Deploy 3.0 installation on local machine.");

                        var installPathValue = key.GetValue("InstallPath");
                        var installPathx64Value = key.GetValue("InstallPath_x64");

                        if(installPathValue == null && installPathx64Value == null)
                        {
                            throw new NotSupportedException("Cannot find WebDeploy installation.");
                        }

                        _webDeployInstallPath = installPathx64Value != null
                                                    ? installPathx64Value.ToString()
                                                    : installPathValue.ToString();
                    }
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
            Exception innerException = null;
            try
            {
                var manScope = GetManagementScope(server);

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
                Logger.Warn(string.Format("Could not find system temp folder (%temp%) on remote server [{0}]. Please check that server exist and provided user have admin rights on server.", server.Name), exception2);
            }
            throw new DirectoryNotFoundException(string.Format("Could not find system temp folder (%temp%) on remote server [{0}]. Please check that server exist and provided user have admin rights on server.", server.Name), innerException);
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
            if (_processObject != null)
            {
                try
                {
                    _processObject.InvokeMethod("Terminate", new object[] {0});
                }
                catch
                {
                    Logger.Warn("Unable to terminate WebDeploy on remote server [{0}].", _server.Name);
                }
                finally
                {
                    _processObject.Dispose();
                    _processObject = null;
                }
            }
            if (_remoteNeedsCleanup)
            {
                using (new Impersonation.Impersonator(_server.DeploymentUserRemote.UserName, _server.DeploymentUserRemote.Password))
                {
                    var retryAttempt = 0;
                    do
                    {
                        try
                        {
                            if (Directory.Exists(RemotePath))
                            {
                                Directory.Delete(RemotePath, true);
                            }
                            _remoteNeedsCleanup = false;
                            Logger.Info(string.Format("Successfully cleaned up WebDeploy files on remote server [{0}]", _server.Name));
                        }
                        catch (UnauthorizedAccessException)
                        {
                            var retryMessage = retryAttempt < RetryAttempts ? "Will retry." : "Retries failed and will abort.";
                            Logger.Verbose(string.Format("Access denied when deleting WebDeploy files on remote server [{0}]. {1}", _server.Name, retryMessage));
                        }
                        catch (IOException)
                        {
                            var retryMessage = retryAttempt < RetryAttempts ? "Will retry." : "Retries failed and will abort.";
                            Logger.Verbose(string.Format("Unable to delete files on remote server [{0}]. {1}", _server.Name, retryMessage));
                        }
                        if (_remoteNeedsCleanup)
                        {
                            if (++retryAttempt > RetryAttempts)
                            {
                                _remoteNeedsCleanup = false;
                                Logger.Warn(string.Format("Failed to cleanup WebDeploy files on remote server [{0}]", _server.Name));
                            }
                            else
                            {
                                Thread.Sleep(RetryInterval*1000);
                            }
                        }
                    } while (_remoteNeedsCleanup);
                }
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