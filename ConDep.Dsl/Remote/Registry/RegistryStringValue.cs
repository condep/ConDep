using System.Management;

namespace ConDep.Dsl.Remote.Registry
{
    public class RegistryStringValue : RegistryValue<string>
    {
        public RegistryStringValue(string server, string username, string password)
            : base(server, username, password, "GetStringValue")
        {
        }

        protected override string DefaultValue
        {
            get { return null; }
        }

        protected override string ConvertValue(ManagementBaseObject method)
        {
            return method.Properties["sValue"].Value as string;
        }
    }
}