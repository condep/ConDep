using System;
using System.Management;

namespace ConDep.Dsl.Remote.Registry
{
    public class RegistryDWordValue : RegistryValue<int>
    {
        public RegistryDWordValue(string server, string username, string password)
            : base(server, username, password, "GetDWORDValue")
        {
        }

        protected override int DefaultValue
        {
            get { return -1; }
        }

        protected override int ConvertValue(ManagementBaseObject regValue)
        {
            return Convert.ToInt32(regValue.Properties["uValue"].Value);
        }
    }
}