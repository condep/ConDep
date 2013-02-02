using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;

namespace ConDep.Dsl.Remote
{
    public class RemoteExecutor
    {
        private string _server;
        private ConnectionOptions _wmiConnectionOptions;

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

        public void Execute(string filePath, string fileParams, bool waitForExit, TimeSpan timeout)
        {
            var options = new ObjectGetOptions();
            var scope = GetScope(_server, _wmiConnectionOptions);
            int processId = 0;
            int result = -1;
            ManagementObject processObject = null;

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

                        if(waitForExit)
                        {
                            WaitForExit(processId, scope, timeout);
                        }
                    }

                    
                }
            }
        }

        private void WaitForExit(int processId, ManagementScope scope, TimeSpan timeout)
        {
            var query = new ObjectQuery("SELECT ProcessId FROM Win32_Process WHERE ProcessId = " + processId);

            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                var isRunning = true;

                var startTime = DateTime.Now;
                while(isRunning)
                {
                    using (ManagementObjectCollection result = searcher.Get())
                    {
                        if (result.Count == 0)
                        {
                            isRunning = false;
                        }
                    }

                    if ((DateTime.Now - startTime) > timeout)
                    {
                        return;
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        private ManagementScope GetScope(string server, ConnectionOptions options)
        {
            var scope = new ManagementScope(string.Format(CultureInfo.InvariantCulture, @"\\{0}\ROOT\CIMV2", server), options);
            scope.Connect();
            return scope;
        }

    }
}