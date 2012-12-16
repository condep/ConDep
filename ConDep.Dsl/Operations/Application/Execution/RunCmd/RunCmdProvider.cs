using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
	public class RunCmdProvider : WebDeployProviderBase
	{
        private const string NAME = "runCommand";

		public RunCmdProvider(string command, bool continueOnError)
		{
		    DestinationPath = command;
		}

	    public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return DeploymentManager.CreateObject(Name, "", sourceBaseOptions);
		}

		public override string Name
		{
			get { return NAME; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
		{
			var destProviderOptions = new DeploymentProviderOptions(Name) { Path = DestinationPath };
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