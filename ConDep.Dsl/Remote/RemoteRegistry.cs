using System.Collections.Generic;
using Microsoft.Win32;

namespace ConDep.Dsl.Remote.Registry
{
    public class RemoteRegistry
    {
        private readonly string _server;
        private readonly string _username;
        private readonly string _password;

        public RemoteRegistry(string server, string username, string password)
        {
            _server = server;
            _username = username;
            _password = password;
        }

        /// <exception cref="System.UnauthorizedAccessException">Thrown when provided user don't have access to the remote registry</exception>
        public bool TryGetDWordValue(RegistryHive hive, string keyName, string valueName, out int value)
        {
            var regValue = new RegistryDWordValue(_server, _username, _password);
            return regValue.TryGetValue(hive, keyName, valueName, out value);
        }

        /// <exception cref="System.UnauthorizedAccessException">Thrown when provided user don't have access to the remote registry</exception>
        public bool TryGetStringValue(RegistryHive hive, string keyName, string valueName, out string value)
        {
            var regValue = new RegistryStringValue(_server, _username, _password);
            return regValue.TryGetValue(hive, keyName, valueName, out value);
        }

        /// <exception cref="System.UnauthorizedAccessException">Thrown when provided user don't have access to the remote registry</exception>
        public bool TryGetBinaryValue(RegistryHive hive, string keyName, string valueName, out byte[] value)
        {
            var regValue = new RegistryBinaryValue(_server, _username, _password);
            return regValue.TryGetValue(hive, keyName, valueName, out value);
        }

        /// <exception cref="System.UnauthorizedAccessException">Thrown when provided user don't have access to the remote registry</exception>
        public bool TryGetExpandedStringValue(RegistryHive hive, string keyName, string valueName, out string value)
        {
            var regValue = new RegistryExpandedStringValue(_server, _username, _password);
            return regValue.TryGetValue(hive, keyName, valueName, out value);
        }

        /// <exception cref="System.UnauthorizedAccessException">Thrown when provided user don't have access to the remote registry</exception>
        public bool TryGetMultiStringValue(RegistryHive hive, string keyName, string valueName, out IEnumerable<string> value)
        {
            var regValue = new RegistryMultiStringValue(_server, _username, _password);
            return regValue.TryGetValue(hive, keyName, valueName, out value);
        }
    }
}