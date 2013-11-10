using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management;
using Microsoft.Win32;

namespace ConDep.Dsl.Remote.Registry
{
    public abstract class RegistryValue<T>
    {
        private readonly string _methodName;
        private string _server;
        private ConnectionOptions _wmiConnectionOptions;

        protected RegistryValue(string server, string username, string password, string methodName)
        {
            _server = server;
            _wmiConnectionOptions = new ConnectionOptions
                                        {
                                            Impersonation = ImpersonationLevel.Impersonate,
                                            EnablePrivileges = true,
                                            Username = username,
                                            Password = password
                                        };
            _methodName = methodName;
        }

        public bool TryGetValue(RegistryHive hive, string keyName, string valueName, out T value)
        {
            value = DefaultValue;
            try
            {
                var scope = GetScope(_server, _wmiConnectionOptions);

                var path = new ManagementPath("stdRegProv");
                using (var registry = new ManagementClass(scope, path, new ObjectGetOptions()))
                {
                    var method = registry.GetMethodParameters(_methodName);
                    method.SetPropertyValue("hDefKey", (uint) hive);
                    method.SetPropertyValue("sSubKeyName", keyName);
                    method.SetPropertyValue("sValueName", valueName);

                    
                    var methodOptions = new InvokeMethodOptions();
                    var returnValue = registry.InvokeMethod(_methodName, method, methodOptions);
                    if (returnValue != null)
                    {
                        value = ConvertValue(returnValue);
                        return true;
                    }
                }
                return false;
            }
            #pragma warning disable 0168
            catch(UnauthorizedAccessException accessException)
            {
                throw;
            }
            #pragma warning restore 0168
            catch
            {
                return false;
            }
        }

        protected abstract T DefaultValue { get; }
        protected abstract T ConvertValue(ManagementBaseObject method);

        private ManagementScope GetScope(string server, ConnectionOptions options)
        {
            var scope = new ManagementScope(string.Format(CultureInfo.InvariantCulture, @"\\{0}\ROOT\default", server), options);
            scope.Connect();
            return scope;
        }

    }
}