using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;
using System.Linq;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Remote
{
    public class RemoteExecutor : IDisposable
    {
        private string _server;
        private ConnectionOptions _wmiConnectionOptions;
        private bool _disposed;
        private ManagementObject _process;

        public RemoteExecutor(string server, string username, string password)
        {
            _server = server;
            _wmiConnectionOptions = new ConnectionOptions
            {
                Impersonation = ImpersonationLevel.Impersonate,
                EnablePrivileges = true,
                Username = username,
                Password = password
            };
        }

        public void StartProcess(string filePath, string fileParams)
        {
            StartProcess(filePath,fileParams, false, TimeSpan.Zero);                        
        }

        public void StartProcess(string filePath, string fileParams, TimeSpan timeout)
        {
            StartProcess(filePath, fileParams, true, timeout);                        
        }

        private void StartProcess(string filePath, string fileParams, bool waitForExit, TimeSpan timeout)
        {
            var options = new ObjectGetOptions();
            var scope = GetScope(_server, _wmiConnectionOptions);
            int processId = 0;
            int result = -1;

            using (var managementClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), options))
            {
                using (ManagementBaseObject managementBaseObject = managementClass.GetMethodParameters("Create"))
                {
                    managementBaseObject["CommandLine"] = filePath + " " + fileParams;
                    using (ManagementBaseObject management = managementClass.InvokeMethod("Create", managementBaseObject, null))
                    {
                        if(management == null)
                            throw new Exception("Unable to create remote process.");

                        result = Convert.ToInt32(management["returnValue"]);

                        switch (result)
                        {
                            case 2:
                            case 3:
                                throw new UnauthorizedAccessException();
                            case 8:
                            case 21:
                                throw new Exception();
                            case 9:
                                throw new FileNotFoundException();
                        }

                        processId = Convert.ToInt32(management["ProcessId"]);

                        var query = new ObjectQuery("SELECT * FROM Win32_Process WHERE ProcessId = " + processId);

                        using (var searcher = new ManagementObjectSearcher(scope, query))
                        {
                            using (ManagementObjectCollection objects = searcher.Get())
                            {
                                foreach (ManagementObject managementObject in objects)
                                {
                                    _process = managementObject;
                                }
                            }
                        }

                        if(_process == null)
                        {
                            throw new Exception("Unable to start remote process.");
                        }

                        if(waitForExit)
                        {
                            WaitForExit(processId, scope, timeout);
                        }
                    }
                }
            }
        }

        private bool IsProcessRunning(int processId, ManagementScope scope)
        {
            if (_process == null) return false;

            var query = new ObjectQuery("SELECT ProcessId FROM Win32_Process WHERE ProcessId = " + processId);

            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection result = searcher.Get())
                {
                    if (result.Count == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private void WaitForExit(int processId, ManagementScope scope, TimeSpan timeout)
        {
            var startTime = DateTime.Now;
            while(IsProcessRunning(processId, scope))
            {
                if ((DateTime.Now - startTime) > timeout)
                {
                    return;
                }
                Thread.Sleep(1000);
            }
        }

        private ManagementScope GetScope(string server, ConnectionOptions options)
        {
            var scope = new ManagementScope(string.Format(CultureInfo.InvariantCulture, @"\\{0}\ROOT\CIMV2", server), options);
            scope.Connect();
            return scope;
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
                if(_process != null)
                {
                    try
                    {
                        _process.InvokeMethod("Terminate", new object[] { 0 });
                    }
                    catch
                    {
                        Logger.Warn("Unable to terminate remote process on [{0}].", _server);
                    }
                    finally
                    {
                        _process.Dispose();
                        _process = null;
                    }
                }
            }
            _disposed = true;
        }

        ~RemoteExecutor()
        {
            Dispose(false);
        }

    }
}