using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.WebDeployProviders.PowerShell
{
    public class PowerShellProvider : WebDeployCompositeProviderBase
    {
        public PowerShellProvider(string command)
        {
            DestinationPath = command;
        }

        public bool ContinueOnError { get; set; }

        public override void Configure(DeploymentServer server)
        {
            Configure<ProvideForInfrastructure>(server, po => po.RunCmd(string.Format(@"powershell.exe -InputFormat none -Command ""& {{ $ErrorActionPreference='stop'; {0}; exit $LASTEXITCODE }}""", DestinationPath), this.ContinueOnError, o => o.WaitIntervalInSeconds(this.WaitInterval)));
        }

        public override bool IsValid(Notification notification)
        {
            return string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}