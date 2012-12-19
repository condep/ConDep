using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Microsoft.Web.Deployment;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class WebDeployTempAgentTests
    {
         [Test]
        public void Test()
         {
             AgentClientProvider();
         }

        public void AgentClientProvider()
    {
      var connectionGroupName = Guid.NewGuid().ToString();
      //baseContext.RaiseEvent((DeploymentTraceEventArgs) new DeploymentAgentTraceEvent(TraceLevel.Info, Resources.UsingRequestIdForConnection, new object[1]
      //{
      //  (object) this._connectionGroupName
      //}));
        using(var tempAgentContext = new TempAgentContext(connectionGroupName))
        {
            
        }
        //try
        //{
        //  int num = 0;
        //  while (true)
        //  {
        //    try
        //    {
        //      this.PerformHeadRequestForAuthentication();
        //      break;
        //    }
        //    catch (Exception ex)
        //    {
        //      if (ex is DeploymentAgentUnavailableException || ex is DeploymentDetailedException && (ex as DeploymentDetailedException).ErrorCode == DeploymentErrorCode.ERROR_DESTINATION_NOT_REACHABLE)
        //      {
        //        if (++num <= this.BaseContext.RetryAttempts)
        //        {
        //          //baseContext.RaiseEvent((DeploymentTraceEventArgs) new DeploymentAgentTraceEvent(TraceLevel.Info, Resources.TempAgentRetrying, new object[1]
        //          //{
        //          //  (object) this.AgentUrl
        //          //}));
        //          Thread.Sleep(this.BaseContext.RetryInterval);
        //        }
        //        else
        //          throw;
        //      }
        //      else
        //        throw;
        //    }
        //  }
        //}
        //catch
        //{
        //  this._tempAgentContext.Dispose();
        //  throw;
        //}
    }

    }
















    internal class TempAgentContext : IDisposable
    {
        private string _agentUrl;
        private static string _assemblyRedirectionConfiguration;
        private string _guid;
        private ManagementScope _manScope;
        private ManagementObject _processObject;
        private string _remoteDestDir;
        private string _remoteDestShare;
        private bool _remoteDestShareNeedsToBeCleanedUp;
        private string _remoteWindowsDir;
        private const string redirectionContent = "\r\n            <configuration>\r\n                <startup  useLegacyV2RuntimeActivationPolicy='true' >\r\n                    <supportedRuntime version='v4.0' />\r\n                    <supportedRuntime version='v2.0.50727' />\r\n                </startup>               \r\n                <runtime>\r\n                  <assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>\r\n                   <dependentAssembly>\r\n                     <assemblyIdentity name='Microsoft.Web.Deployment'\r\n                                       publicKeyToken='31bf3856ad364e35'\r\n                                       culture='neutral' />\r\n                     <bindingRedirect oldVersion='{0}'\r\n                                      newVersion='{1}' />\r\n                   </dependentAssembly>\r\n                  </assemblyBinding>\r\n               </runtime>\r\n            </configuration>";

        // Methods
        public TempAgentContext(string guid)
        {
            //this._baseContext = baseContext;
            _guid = guid;
            try
            {
                Initialize();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        private void CreateRemoteDirectories()
        {
            DirectoryInfo info;
            var directorySecurity = new DirectorySecurity();
            directorySecurity.SetSecurityDescriptorSddlForm("D:PAI(A;;FA;;;BA)");

            try
            {
                info = Directory.CreateDirectory(RemoteDestShare, directorySecurity);
                _remoteDestShareNeedsToBeCleanedUp = true;
            }
            catch (IOException exception)
            {
                //if (COMHelper.IsExceptionSameAsError(exception, (ErrorCode)(-2147023570)))
                //{
                //    throw new DeploymentException(exception, Resources.TempAgentNeedsRunAsAdminOrNetUse, new object[] { this.RemoteDestShare, this.BaseContext.LocationInfo.AffectiveUserName, this.BaseContext.LocationInfo.ComputerName, this.RemoteWindowsShare });
                //}
                throw;
            }
            
            string[] allDirs = new string[3]
            {
              "x86",
              "x64",
              "Extensibility"
            };

            foreach (string str in allDirs)
            {
                info.CreateSubdirectory(str);
            }
        }

        public void Dispose()
        {
            if (this._processObject != null)
            {
                try
                {
                    var args = new object[] { 0 };
                    //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.TerminatingTempAgentProcess, new object[] { obj2 }));
                    _processObject.InvokeMethod("Terminate", args);
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
            if (_remoteDestShareNeedsToBeCleanedUp)
            {
                //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.DeletingTempAgentDirectory, new object[] { this.RemoteDestShare }));
                int retryAttempt = 0;
                do
                {
                    Exception exception2 = new IOException();
                    try
                    {
                        if (Directory.Exists(RemoteDestShare))
                        {
                            Directory.Delete(RemoteDestShare, true);
                        }
                        _remoteDestShareNeedsToBeCleanedUp = false;
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
                    if (this._remoteDestShareNeedsToBeCleanedUp)
                    {
                        if (++retryAttempt > RetryAttempts)
                        {
                            //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(TraceLevel.Warning, Resources.DeleteElementErrorMessage, new object[] { this.RemoteDestShare, Resources.DirPathProviderDescription, exception2 }));
                            _remoteDestShareNeedsToBeCleanedUp = false;
                        }
                        else
                        {
                            //this.BaseContext.RaiseEvent(new DeploymentRetryEventArgs(exception2.Message, DeploymentOperationKind.Delete, "dirPath", this.RemoteDestShare, retryAttempt, this.BaseContext.RetryAttempts));
                            Thread.Sleep(RetryInterval);
                        }
                    }
                }
                while (_remoteDestShareNeedsToBeCleanedUp);
            }
        }


        protected int RetryInterval { get; set; }

        protected int RetryAttempts { get; set; }

        private static void FileCopy(string source, string dest)
        {
            string str;
            //baseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.CopyingTempAgentFile, new object[] { source, dest }));
            Copy(source, dest, false);
            if (TryGetSymbolFile(source, out str))
            {
                string fileName = Path.GetFileName(str);
                string destPath = Combine(Path.GetDirectoryName(dest), fileName);
                Copy(str, destPath, false);
            }
        }

        public static void Copy(string sourcePath, string destPath, bool overwrite)
        {
            if (CopyFileW(GetCanonicalizedForm(sourcePath), GetCanonicalizedForm(destPath), !overwrite))
                return;
            RaiseIOExceptionFromGetLastError();
        }

        public static string GetCanonicalizedForm(string path)
        {
            if (!IsAbsolutePhysicalPath(path))
                path = Path.Combine(Environment.CurrentDirectory, path);
            path = path.Replace('/', '\\');
            if (!path.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
                return "\\\\?\\" + path;
            if (path.Length >= 4 && (int)path[3] == 92)
            {
                if ((int)path[2] == 63)
                    return path;
                if ((int)path[3] == 46)
                    return "\\\\?" + path.Substring(3);
            }
            return "\\\\?\\UNC\\" + path.Substring(2);
        }

        public static bool IsAbsolutePhysicalPath(string path)
        {
            return !string.IsNullOrEmpty(path) && path.Length >= 3 && ((int)path[1] == (int)Path.VolumeSeparatorChar && IsDirectorySeparatorChar(path[2]) || IsDirectorySeparatorChar(path[0]) && IsDirectorySeparatorChar(path[1]));
        }

        public static bool IsDirectorySeparatorChar(char c)
        {
            return (int)c == (int)Path.DirectorySeparatorChar || (int)c == (int)Path.AltDirectorySeparatorChar;
        }



        internal static void RaiseIOExceptionFromGetLastError()
        {
            RaiseIOExceptionFromGetLastError(string.Empty);
        }

        internal static void RaiseIOExceptionFromGetLastError(string path)
        {
            RaiseIOExceptionFromErrorCode((Win32ErrorCode)Marshal.GetLastWin32Error(), path);
        }

        internal static void RaiseIOExceptionFromErrorCode(Win32ErrorCode errorCode, string maybeFullPath)
        {
            switch (errorCode)
            {
                case Win32ErrorCode.ERROR_FILENAME_EXCED_RANGE:
                    throw new PathTooLongException("PathTooLong");
                case Win32ErrorCode.ERROR_OPERATION_ABORTED:
                    throw new OperationCanceledException();
                case Win32ErrorCode.ERROR_JOURNAL_ENTRY_DELETED:
                    throw new IOException("JournalEntryPurged", MakeHRFromErrorCode(errorCode));
                case Win32ErrorCode.ERROR_INVALID_NAME:
                    throw new IOException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "IO_Invalid_Name", new object[1]
          {
            (object) maybeFullPath
          }), MakeHRFromErrorCode(errorCode));
          //      case Win32ErrorCode.ERROR_ALREADY_EXISTS:
          //          if (maybeFullPath.Length != 0)
          //              throw new IOException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "IO_AlreadyExists_Name", new object[1]
          //  {
          //    (object) maybeFullPath
          //  }), MakeHRFromErrorCode(errorCode));
          //          else
          //              break;
          //      case Win32ErrorCode.ERROR_SHARING_VIOLATION:
          //          if (maybeFullPath.Length == 0)
          //              throw new IOException("IO_SharingViolation_NoFileName", MakeHRFromErrorCode(errorCode));
          //          throw new IOException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "IO_SharingViolation_File", new object[1]
          //{
          //  (object) maybeFullPath
          //}), MakeHRFromErrorCode(errorCode));
          //      case Win32ErrorCode.ERROR_FILE_EXISTS:
          //          if (maybeFullPath.Length != 0)
          //              throw new IOException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "IO_FileExists_Name", new object[1]
          //  {
          //    (object) maybeFullPath
          //  }), MakeHRFromErrorCode(errorCode));
          //          else
          //              break;
          //      case Win32ErrorCode.ERROR_INVALID_PARAMETER:
          //          throw new IOException(NativeMethods.GetMessage(errorCode), NativeMethods.MakeHRFromErrorCode(errorCode));
          //      case NativeMethods.Win32ErrorCode.ERROR_FILE_NOT_FOUND:
          //          if (maybeFullPath.Length == 0)
          //              throw new FileNotFoundException(Resources.FileNotFound);
          //          throw new FileNotFoundException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, Resources.FileNotFound_FileName, new object[1]
          //{
          //  (object) maybeFullPath
          //}), maybeFullPath);
          //      case NativeMethods.Win32ErrorCode.ERROR_PATH_NOT_FOUND:
          //          if (maybeFullPath.Length == 0)
          //              throw new DirectoryNotFoundException(Resources.PathNotFound_NoPathName);
          //          throw new DirectoryNotFoundException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, Resources.PathNotFound_Path, new object[1]
          //{
          //  (object) maybeFullPath
          //}));
          //      case NativeMethods.Win32ErrorCode.ERROR_ACCESS_DENIED:
          //          if (maybeFullPath.Length == 0)
          //              throw new UnauthorizedAccessException(Resources.UnauthorizedAccess_IODenied_NoPathName);
          //          throw new UnauthorizedAccessException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, Resources.UnauthorizedAccess_IODenied_Path, new object[1]
          //{
          //  (object) maybeFullPath
          //}));
          //      case NativeMethods.Win32ErrorCode.ERROR_INVALID_DRIVE:
          //          throw new DriveNotFoundException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, Resources.DriveNotFound_Drive, new object[1]
          //{
          //  (object) maybeFullPath
          //}));
            }
            throw new IOException(errorCode.ToString());
        }

        internal static int MakeHRFromErrorCode(Win32ErrorCode errorCode)
        {
            return (int)(((Win32ErrorCode)(-2147024896)) | errorCode);
        }

      //  internal static string GetMessage(Win32ErrorCode errorCode)
      //  {
      //      StringBuilder lpBuffer = new StringBuilder(512);
      //      if (FormatMessageW((NativeMethods.FormatMessageFlags)12800, IntPtr.Zero, (int)errorCode, 0, lpBuffer, lpBuffer.Capacity, IntPtr.Zero) != 0)
      //          return ((object)lpBuffer).ToString();
      //      return string.Format((IFormatProvider)CultureInfo.CurrentCulture, Resources.UnknownError_Num, new object[1]
      //{
      //  (object) errorCode
      //});
      //  }


        public enum Win32ErrorCode
        {
            ERROR_ACCESS_DENIED = 5,
            ERROR_ALREADY_EXISTS = 0xb7,
            ERROR_BAD_PATHNAME = 0xa1,
            ERROR_FILE_EXISTS = 80,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_FILENAME_EXCED_RANGE = 0xce,
            ERROR_INSUFFICIENT_BUFFER = 0x7a,
            ERROR_INVALID_DRIVE = 15,
            ERROR_INVALID_HANDLE = 6,
            ERROR_INVALID_NAME = 0x7b,
            ERROR_INVALID_PARAMETER = 0x57,
            ERROR_JOURNAL_ENTRY_DELETED = 0x49d,
            ERROR_NO_MORE_FILES = 0x12,
            ERROR_NOT_READY = 0x15,
            ERROR_OPERATION_ABORTED = 0x3e3,
            ERROR_PATH_NOT_FOUND = 3,
            ERROR_SHARING_VIOLATION = 0x20
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CopyFileW(string sourcePath, string destPath, [MarshalAs(UnmanagedType.Bool)] bool failIfExists);




        private static ManagementClass GetManagementClass(ManagementScope manScope, string className)
        {
            ObjectGetOptions options = new ObjectGetOptions();
            return new ManagementClass(manScope, new ManagementPath(className), options);
        }

        private static ManagementScope GetManagementScope(string username, string password, string computerNameWithoutPort)
        {
            ConnectionOptions options = new ConnectionOptions
            {
                Impersonation = ImpersonationLevel.Impersonate,
                EnablePrivileges = true
            };
            if (!string.IsNullOrEmpty(username))
            {
                options.Username = username;
                options.Password = password;
            }
            ManagementScope scope = new ManagementScope(FormatStringInvariant(@"\\{0}\ROOT\CIMV2", new object[] { computerNameWithoutPort }), options);
            try
            {
                scope.Connect();
            }
            catch (COMException exception)
            {
                //if (COMHelper.IsExceptionSameAsError(exception, (ErrorCode)(-2147023174)))
                //{
                    throw new DeploymentFatalException(exception, "CannotConnectToRemoteWMI", new object[] { computerNameWithoutPort });
                //}
            }
            return scope;
        }

        public static string FormatStringInvariant(string formatString, params object[] parameters)
        {
            return string.Format((IFormatProvider)CultureInfo.InvariantCulture, formatString, parameters);
        }

        private static string GetRemoteWindowsFolder(ManagementScope manScope, string computerName)
        {
            Exception innerException = null;
            try
            {
                using (ManagementClass class2 = GetManagementClass(manScope, "Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj2 in class2.GetInstances())
                    {
                        using (obj2)
                        {
                            string propertyValue = obj2.GetPropertyValue("WindowsDirectory") as string;
                            if (!string.IsNullOrEmpty(propertyValue))
                            {
                                return propertyValue;
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                innerException = exception2;
            }
            throw new DeploymentFatalException(innerException, "CannotGetRemoteEnvironmentVariable", new object[] { "WindowsDirectory", computerName });
        }

        private void Initialize()
        {
            string mSDeployInstallOrBinPath;
            Username = "";
            Password = "";
            RetryAttempts = 5;
            RetryInterval = 5;

            //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.CreatingTempAgentDirectory, new object[] { this.RemoteDestShare }));
            this.CreateRemoteDirectories();

            
            List<CopyFileInfo> list = new List<CopyFileInfo>();
            string mSDeployInstallPath = @"C:\Program Files\IIS\Microsoft Web Deploy V3\";// AssemblyEnvironment.MSDeployInstallPath;
            string sourcePath = @"C:\Program Files\IIS\Microsoft Web Deploy\";// AssemblyEnvironment.MSDeployV1InstallPath;
            //if (!string.IsNullOrEmpty(mSDeployInstallPath))
            //{
                mSDeployInstallOrBinPath = mSDeployInstallPath;
                list.Add(new CopyFileInfo(sourcePath, "MsDepSvc.exe"));
            //}
            //else
            //{
            //    mSDeployInstallOrBinPath = AssemblyEnvironment.MSDeployInstallOrBinPath;
            //    list.Add(new CopyFileInfo(mSDeployInstallOrBinPath, "MsDepSvc.exe"));
            //}
            list.Add(new CopyFileInfo(mSDeployInstallOrBinPath, "Microsoft.Web.Deployment.dll"));
            list.Add(new CopyFileInfo(mSDeployInstallOrBinPath, "Microsoft.Web.Delegation.dll"));
            list.Add(new CopyFileInfo(mSDeployInstallOrBinPath, @"x64\axnative.dll"));//AxNative.GoodAxNativeDllPath));
            list.Add(new CopyFileInfo(mSDeployInstallOrBinPath, @"x86\axnative.dll"));//AxNative.BadAxNativeDllPath));
            foreach (CopyFileInfo info in list)
            {
                FileCopy(Combine(info.SourcePath, info.SourceName), Combine(this.RemoteDestShare, info.DestinationName));
            }
            using (var writer = new StreamWriter(Combine(RemoteDestShare, "MsDepSvc.exe.config")))
            {
                writer.WriteLine(AssemblyRedirectionConfiguration);
            }
            //string basePath = Combine(this.RemoteDestShare, "Extensibility");
            //foreach (string str5 in GetExtensiblityAssemblyFiles(true, mSDeployInstallOrBinPath))
            //{
            //    string dest = Combine(basePath, Path.GetFileName(str5));
            //    FileCopy(this.BaseContext, str5, dest);
            //}
            using (ManagementClass class2 = GetManagementClass(this.ManagementScope, "Win32_Process"))
            {
                using (ManagementBaseObject obj2 = class2.GetMethodParameters("Create"))
                {
                    string str10;
                    string port = "80";// this.BaseContext.LocationInfo.Port;
                    string str8 = "http://+:" + port + "/" + this.Guid + "/";
                    this._agentUrl = "http://" + ComputerNameWithoutPort + ":" + port + "/" + this.Guid;
                    string str9 = Combine(this.RemoteDestDir, "MsDepSvc.exe") + " -listenUrl:" + str8;
                    obj2["CommandLine"] = str9;
                    //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.RunningRemoteTempAgentCommandLine, new object[] { str9 }));
                    using (ManagementBaseObject obj3 = class2.InvokeMethod("Create", obj2, null))
                    {
                        str10 = obj3["ProcessId"].ToString();
                    }
                    //this.BaseContext.RaiseEvent(new DeploymentAgentTraceEvent(Resources.DeterminedTempAgentProcessId, new object[] { str10 }));
                    string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSDEPLOYBREAK"));
                    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Process WHERE ProcessId = " + str10);
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.ManagementScope, query))
                    {
                        using (ManagementObjectCollection objects = searcher.Get())
                        {
                            foreach (ManagementObject obj4 in objects)
                            {
                                this._processObject = obj4;
                            }
                        }
                    }

                }
            }
        }

        protected string ComputerNameWithoutPort { get { return "jat-web03"; } }
        protected string ComputerName { get { return "jat-web03:80"; } }

        //private static IEnumerable<string> GetExtensiblityAssemblyFiles(bool fullName, string installPath)
        //{
        //    string extensibilityFolderPath = Combine(installPath, "Extensibility");
        //    if (DirectoryEx.Exists(extensibilityFolderPath))
        //    {
        //        DirectoryInfoEx extensibilityFolder = new DirectoryInfoEx(extensibilityFolderPath);
        //        foreach (FileInfoEx fileInfoEx in extensibilityFolder.GetFiles())
        //        {
        //            if (!string.IsNullOrEmpty(fileInfoEx.Name) && fileInfoEx.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        //            {
        //                if (fullName)
        //                    yield return fileInfoEx.FullName;
        //                else
        //                    yield return fileInfoEx.Name;
        //            }
        //        }
        //    }
        //}


        private static bool TryGetSymbolFile(string sourceFilePath, out string path)
        {
            string directoryName = Path.GetDirectoryName(sourceFilePath);
            string subPath = Path.GetFileNameWithoutExtension(sourceFilePath) + ".pdb";
            string str3 = Combine(directoryName, subPath);
            if (Exists(str3))
            {
                path = str3;
                return true;
            }
            string extension = Path.GetExtension(sourceFilePath);
            if (extension.StartsWith(".", StringComparison.Ordinal))
            {
                extension = extension.Substring(1);
            }
            string str5 = Combine(Combine(Combine(directoryName, "symbols"), extension), subPath);
            if (Exists(str5))
            {
                path = str5;
                return true;
            }
            path = string.Empty;
            return false;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern FileAttributes GetFileAttributesW(string lpFileName);

        public static bool Exists(string path)
        {
            try
            {
                path = GetCanonicalizedForm(path);
                FileAttributes fileAttributesW = GetFileAttributesW(path);
                return ((((int)fileAttributesW) != -1) && ((fileAttributesW & FileAttributes.Directory) == 0));
            }
            catch
            {
                return false;
            }
        }

 


        // Properties
        public string AgentUrl
        {
            get
            {
                return this._agentUrl;
            }
        }

        private static string AssemblyRedirectionConfiguration
        {
            get
            {
                if (string.IsNullOrEmpty(_assemblyRedirectionConfiguration))
                {
                    _assemblyRedirectionConfiguration = string.Format("\r\n            <configuration>\r\n                <startup  useLegacyV2RuntimeActivationPolicy='true' >\r\n                    <supportedRuntime version='v4.0' />\r\n                    <supportedRuntime version='v2.0.50727' />\r\n                </startup>               \r\n                <runtime>\r\n                  <assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>\r\n                   <dependentAssembly>\r\n                     <assemblyIdentity name='Microsoft.Web.Deployment'\r\n                                       publicKeyToken='31bf3856ad364e35'\r\n                                       culture='neutral' />\r\n                     <bindingRedirect oldVersion='{0}'\r\n                                      newVersion='{1}' />\r\n                   </dependentAssembly>\r\n                  </assemblyBinding>\r\n               </runtime>\r\n            </configuration>", "7.1.0.0", "9.0.0.0");
                }
                return _assemblyRedirectionConfiguration;
            }
        }

        //private DeploymentBaseContext BaseContext
        //{
        //    get
        //    {
        //        return this._baseContext;
        //    }
        //}

        private string Guid
        {
            get
            {
                return this._guid;
            }
        }

        private ManagementScope ManagementScope
        {
            get
            {
                if (this._manScope == null)
                {
                    this._manScope = GetManagementScope(Username, Password, ComputerNameWithoutPort);
                }
                return this._manScope;
            }
        }

        protected string Password { get; set; }

        protected string Username { get; set; }

        private string RemoteDestDir
        {
            get
            {
                if (string.IsNullOrEmpty(this._remoteDestDir))
                {
                    string basePath = Combine(this.RemoteWindowsDir, "TEMP");
                    this._remoteDestDir = Combine(Combine(basePath, "MSDEPLOY"), this._guid);
                }
                return this._remoteDestDir;
            }
        }

        private string RemoteDestShare
        {
            get
            {
                if (string.IsNullOrEmpty(this._remoteDestShare))
                {
                    this._remoteDestShare = ConvertToRemotePath(this.RemoteDestDir);
                }
                return this._remoteDestShare;
            }
        }

        public string ConvertToRemotePath(string localPath)
        {
            string computerNameWithoutPort = this.ComputerNameWithoutPort;
            if (string.IsNullOrEmpty(computerNameWithoutPort) || !char.IsLetter(localPath[0]) || (int)localPath[1] != 58)
                return localPath;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\\\\");
            stringBuilder.Append(computerNameWithoutPort);
            stringBuilder.Append("\\");
            stringBuilder.Append(localPath[0]);
            stringBuilder.Append("$");
            stringBuilder.Append(localPath.Substring(2));
            return ((object)stringBuilder).ToString();
        }


        private string RemoteWindowsDir
        {
            get
            {
                if (string.IsNullOrEmpty(this._remoteWindowsDir))
                {
                    this._remoteWindowsDir = GetRemoteWindowsFolder(this.ManagementScope, ComputerName);
                }
                return this._remoteWindowsDir;
            }
        }

        //private string RemoteWindowsShare
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(this._remoteWindowsShare))
        //        {
        //            this._remoteWindowsShare = this.BaseContext.LocationInfo.ConvertToRemotePath(this.RemoteWindowsDir);
        //        }
        //        return this._remoteWindowsShare;
        //    }
        //}

        public static string Combine(string basePath, string subPath)
        {
            if (string.IsNullOrEmpty(subPath))
                return basePath;
            if (string.IsNullOrEmpty(basePath))
                return subPath;
            subPath = subPath.Replace('/', '\\');
            basePath = TerminateWithWhack(basePath);
            subPath = subPath.TrimStart(new char[1]
      {
        '\\'
      });
            return basePath + subPath;
        }

        public static string TerminateWithWhack(string input)
        {
            return TerminateWithString(input, "\\");
        }

        private static string TerminateWithString(string input, string end)
        {
            if (string.IsNullOrEmpty(input))
                return end;
            if (input.EndsWith(end, StringComparison.OrdinalIgnoreCase))
                return input;
            else
                return input + end;
        }


        // Nested Types
        private class CopyFileInfo
        {
            // Fields
            private string _destinationName;
            private string _sourceName;
            private string _sourcePath;

            // Methods
            public CopyFileInfo(string sourcePath, string sourceName)
                : this(sourcePath, sourceName, sourceName)
            {
            }

            public CopyFileInfo(string sourcePath, string sourceName, string destinationName)
            {
                this._sourcePath = sourcePath;
                this._sourceName = sourceName;
                this._destinationName = destinationName;
            }

            // Properties
            public string DestinationName
            {
                get
                {
                    return this._destinationName;
                }
                private set
                {
                    this._destinationName = value;
                }
            }

            public string SourceName
            {
                get
                {
                    return this._sourceName;
                }
                private set
                {
                    this._sourceName = value;
                }
            }

            public string SourcePath
            {
                get
                {
                    return this._sourcePath;
                }
                private set
                {
                    this._sourcePath = value;
                }
            }
        }
    }



}