using ConDep.Dsl.Config;
using ConDep.Dsl.Remote;

namespace ConDep.Dsl.SemanticModel
{
    internal class DotNetFrameworkHarvester : IHarvestServerInfo
    {
        public void Harvest(ServerConfig server)
        {
            var psExecutor = new PowerShellExecutor(server) {LogOutput = false, LoadConDepModule = false};
            var result = psExecutor.Execute(@"$regKeys = @(
        ""HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client"", 
        ""HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"", 
        ""HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5"",
        ""HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0"",
        ""HKLM:\Software\Microsoft\NET Framework Setup\NDP\v2.0.50727"",
        ""HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322""
        )

$result = @()

foreach($regKeyPath in $regKeys) {
    if(test-path $regKeyPath) {

        $regKey = Get-Item $regKeyPath
        $installed = $regKey.GetValue(""Install"")
        if($installed) {
            $dotNetVersion = @{}
            $dotNetVersion.Installed = $installed
            $dotNetVersion.Version = $regKey.GetValue(""Version"")
            $dotNetVersion.ServicePack = $regKey.GetValue(""SP"")
            $dotNetVersion.Release = $regKey.GetValue(""Release"")
            $dotNetVersion.TargetVersion = $regKey.GetValue(""TargetVersion"")
            $dotNetVersion.Client = $regKey.Name.ToLower().EndsWith(""client"")
            $dotNetVersion.Full = $regKey.Name.ToLower().EndsWith(""full"")

            $result += ,@($dotNetVersion)
        }
    }
}

return $result");
            foreach (var element in result)
            {
                server.ServerInfo.DotNetFrameworks.Add(element);
            }
        }
    }
}