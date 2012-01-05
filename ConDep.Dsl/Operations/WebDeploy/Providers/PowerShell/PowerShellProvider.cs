using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
    public class PowerShellProvider : CompositeProvider
    {
        public PowerShellProvider(string command)
        {
            DestinationPath = command;
        }

        public override void Configure()
        {
            Configure(p => p.RunCmd(string.Format(@"powershell.exe -NonInteractive -InputFormat none -Command $ErrorActionPreference='stop'; {0}; exit $LASTEXITCODE", DestinationPath)));
        }

        public override bool IsValid(Notification notification)
        {
            return string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}