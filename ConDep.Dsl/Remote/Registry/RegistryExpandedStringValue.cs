using System.Management;

namespace ConDep.Dsl.Remote.Registry
{
    public class RegistryExpandedStringValue : RegistryValue<string>
    {
        public RegistryExpandedStringValue(string server, string username, string password)
            : base(server, username, password, "GetExpandedStringValue")
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