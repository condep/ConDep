using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
	public class RunCmdProvider : WebDeployProviderBase
	{
        private const string NAME = "runCommand";

		public RunCmdProvider(string command)
		{
		    DestinationPath = command;
		    WaitIntervalInSeconds = 60;
		}

        public override DeploymentProviderOptions GetWebDeploySourceProviderOptions()
        {
            return new DeploymentProviderOptions(NAME) { Path = DestinationPath };
        }

    	public override string Name
		{
			get { return NAME; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
		    var destProviderOptions = new DeploymentProviderOptions("Auto");// { Path = DestinationPath };
            //DeploymentProviderSetting dontUseCmdExe;
            //if (destProviderOptions.ProviderSettings.TryGetValue("dontUseCommandExe", out dontUseCmdExe))
            //{
            //    dontUseCmdExe.Value = true;
            //}
		    return destProviderOptions;
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(DestinationPath) ||
                string.IsNullOrWhiteSpace(SourcePath);
		}
	}
}