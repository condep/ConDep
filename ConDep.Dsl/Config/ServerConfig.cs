using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.Config
{
    public enum DotNetVersion
    {
        v4_5_full,
        v4_5_client,
        v4_5,
        v4_0_full,
        v4_0_client,
        v4_0,
        v3_5,
        v3_0,
        v2_0,
        v1_1
    }
    public class ServerConfig
    {
        private DeploymentUserConfig _deploymentUserRemote;
        private string _agentUrl;
        private ServerInfo _serverInfo = new ServerInfo();

        public string Name { get; set; }
        public bool StopServer { get; set; }
        public IList<WebSiteConfig> WebSites { get; set; }
        public DeploymentUserConfig DeploymentUser 
        { 
            get { return _deploymentUserRemote ?? (_deploymentUserRemote = new DeploymentUserConfig()); }
            set { _deploymentUserRemote = value; }
        }

        public string WebDeployAgentUrl { 
            get
            {
                if(string.IsNullOrEmpty(_agentUrl))
                {
                    _agentUrl = Name;
                }
                return _agentUrl;
            }
            set
            {
                _agentUrl = value;
            }
        }

        public string LoadBalancerFarm { get; set; }
        public string TempFolderPowerShell { get; set; }
        public string TempFolderDos { get; set; }

        public ServerInfo ServerInfo
        {
            get { return _serverInfo; }
        }
    }

    public class ServerInfo
    {
        private readonly DotNetFrameworks _dotNetFrameworks = new DotNetFrameworks();
        private readonly IList<NetworkInfo> _network = new List<NetworkInfo>();
        private readonly IList<DiskInfo> _disks = new List<DiskInfo>(); 

        public DotNetFrameworks DotNetFrameworks { get { return _dotNetFrameworks; } }
        public OperatingSystemInfo OperatingSystem { get; set; }

        public IList<NetworkInfo> Network
        {
            get { return _network; }
        }

        public IList<DiskInfo> Disks
        {
            get { return _disks; }
        }
    }

    public class DiskInfo
    {
        public string DeviceId { get; set; }
        public long SizeInKb { get; set; }
        public long FreeSpaceInKb { get; set; }
        public string Name { get; set; }
        public string FileSystem { get; set; }
        public string VolumeName { get; set; }

        public long SizeInMb { get { return SizeInKb / 1024; } }
        public int SizeInGb { get { return Convert.ToInt32(SizeInMb) / 1024; } }
        public int SizeInTb { get { return Convert.ToInt32(SizeInGb) / 1024; } }

        public long FreeSpaceInMb { get { return FreeSpaceInKb / 1024; } }
        public int FreeSpaceInGb { get { return Convert.ToInt32(FreeSpaceInMb) / 1024; } }
        public int FreeSpaceInTb { get { return Convert.ToInt32(FreeSpaceInGb) / 1024; } }

        public long UsedInKb { get { return SizeInKb - FreeSpaceInKb; } }
        public long UsedInMb { get { return UsedInKb / 1024; } }
        public int UsedInGb { get { return Convert.ToInt32(UsedInMb) / 1024; } }
        public int UsedInTb { get { return Convert.ToInt32(UsedInGb) / 1024; } }

        public int PercentUsed { get { return SizeInKb == 0 ? 0 : Convert.ToInt32(UsedInKb * 100 / SizeInKb); } }
        public int PercentFree { get { return SizeInKb == 0 ? 0 : 100 - PercentUsed; } }
    }

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

    public class OperatingSystemInfo
    {
        public TimeSpan SystemUpTime { get; set; }
        public string HostName { get; set; }
        public string SystemType { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
    }

    public class DotNetFrameworks : IEnumerable<DotNetFrameworkVersion>
    {
        private List<DotNetFrameworkVersion> _versions = new List<DotNetFrameworkVersion>(); 
        public void Add(dynamic dotNetVersion)
        {
            _versions.Add(new DotNetFrameworkVersion
                              {
                                  Installed = Convert.ToBoolean(dotNetVersion.Installed),
                                  Version = dotNetVersion.Version,
                                  ServicePack = dotNetVersion.ServicePack,
                                  Release = dotNetVersion.Release,
                                  TargetVersion = dotNetVersion.TargetVersion,
                                  Client = dotNetVersion.Client,
                                  Full = dotNetVersion.Full
                              });
            var version = dotNetVersion.Version;

        }

        public DotNetFrameworkVersion this[DotNetVersion version]
        {
            get
            {
                switch (version)
                {
                    case DotNetVersion.v1_1:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("1.1"));
                    case DotNetVersion.v2_0:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("2.0"));
                    case DotNetVersion.v3_0:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("3.0"));
                    case DotNetVersion.v3_5:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("3.5"));
                    case DotNetVersion.v4_0:
                        return _versions.SingleOrDefault(x => x.TargetVersion == "4.0.0");
                    case DotNetVersion.v4_0_client:
                        return _versions.SingleOrDefault(x => x.TargetVersion == "4.0.0" && x.Client);
                    case DotNetVersion.v4_0_full:
                        return _versions.SingleOrDefault(x => x.TargetVersion == "4.0.0" && x.Full);
                    case DotNetVersion.v4_5:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("4.5"));
                    case DotNetVersion.v4_5_client:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("4.5") && x.Client);
                    case DotNetVersion.v4_5_full:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("4.5") && x.Full);
                    default:
                        return null;
                }
            }
        } 

        public bool HasVersion(DotNetVersion version)
        {
            return this[version] != null;
        }

        public IEnumerator<DotNetFrameworkVersion> GetEnumerator()
        {
            return _versions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class DotNetFrameworkVersion
    {
        public bool Installed { get; set; }

        public string Version { get; set; }

        public int? ServicePack { get; set; }

        public int? Release { get; set; }

        public string TargetVersion { get; set; }

        public bool Client { get; set; }

        public bool Full { get; set; }
    }
}