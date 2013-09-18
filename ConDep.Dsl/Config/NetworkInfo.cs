using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class NetworkInfo
    {
        public IEnumerable<string> IPAddresses { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> DefaultGateways { get; set; }

        public bool DHCPEnabled { get; set; }

        public string DNSDomain { get; set; }

        public string DNSHostName { get; set; }

        public int Index { get; set; }

        public int InterfaceIndex { get; set; }

        public IEnumerable<string> IPSubnets { get; set; }
    }
}