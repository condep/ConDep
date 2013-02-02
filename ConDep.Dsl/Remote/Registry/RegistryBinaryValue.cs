using System.Management;

namespace ConDep.Dsl.Remote.Registry
{
    public class RegistryBinaryValue : RegistryValue<byte[]>
    {
        public RegistryBinaryValue(string server, string username, string password)
            : base(server, username, password, "GetBinaryValue")
        {
        }

        protected override byte[] DefaultValue
        {
            get { return null; }
        }

        protected override byte[] ConvertValue(ManagementBaseObject method)
        {
            return method.Properties["uValue"].Value as byte[];
        }
    }
}