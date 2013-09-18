using System.Collections.Generic;

namespace ConDep.Dsl.Config
{
    public class ServerInfo
    {
        private readonly DotNetFrameworks _dotNetFrameworks = new DotNetFrameworks();
        private readonly IList<NetworkInfo> _network = new List<NetworkInfo>();
        private readonly IList<DiskInfo> _disks = new List<DiskInfo>(); 

        public DotNetFrameworks DotNetFrameworks { get { return _dotNetFrameworks; } }
        public OperatingSystemInfo OperatingSystem { get; set; }

        public string TempFolderPowerShell { get; set; }

        public string TempFolderDos { get; set; }

        public IList<NetworkInfo> Network
        {
            get { return _network; }
        }

        public IList<DiskInfo> Disks
        {
            get { return _disks; }
        }
    }
}