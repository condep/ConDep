using System.Collections.Generic;
using System.Management;

namespace ConDep.Dsl.Remote.Registry
{
    public class RegistryMultiStringValue : RegistryValue<IEnumerable<string>>
    {
        public RegistryMultiStringValue(string server, string username, string password)
            : base(server, username, password, "GetMultiStringValue")
        {
        }

        protected override IEnumerable<string> DefaultValue
        {
            get { return new string[]{}; }
        }

        protected override IEnumerable<string> ConvertValue(ManagementBaseObject method)
        {
            return method.Properties["sValue"].Value as string[];
        }
    }
}