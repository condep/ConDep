using System;
using System.Linq;
using ConDep.Dsl.Config;
using ConDep.Dsl.Remote;

namespace ConDep.Dsl.SemanticModel
{
    internal class OperatingSystemHarvester : IHarvestServerInfo
    {
        public void Harvest(ServerConfig server)
        {
            var psExecutor = new PowerShellExecutor(server) { LogOutput = false, LoadConDepModule = false };
            var osInfo = @"$perfData = Get-WmiObject win32_perfformatteddata_perfos_system -Property SystemUpTime
$compSystem = Get-WmiObject win32_computersystem -Property Name,SystemType
$os = Get-WmiObject win32_operatingsystem -Property Caption,Version,BuildNumber

$osInfo = @{}
$osInfo.SystemUpTime = $perfData.SystemUpTime
$osInfo.HostName = $compSystem.Name
$osInfo.SystemType = $compSystem.SystemType
$osInfo.Name = $os.Caption
$osInfo.Version = $os.Version
$osInfo.BuildNumber = $os.BuildNumber

return $osInfo
";

            var osInfoResult = psExecutor.Execute(osInfo).FirstOrDefault();
            if (osInfoResult != null)
            {
                server.GetServerInfo().OperatingSystem = new OperatingSystemInfo
                                                        {
                                                            BuildNumber = osInfoResult.BuildNumber,
                                                            Name = osInfoResult.Name,
                                                            HostName = osInfoResult.HostName,
                                                            SystemType = osInfoResult.SystemType,
                                                            SystemUpTime = TimeSpan.FromSeconds(Convert.ToDouble(osInfoResult.SystemUpTime)),
                                                            Version = osInfoResult.Version
                                                        };
            }

        }
    }
}