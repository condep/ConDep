using ConDep.Dsl.FluentWebDeploy.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy
{
	public class RunCmdProvider : Provider
	{
		private const string NAME = "runCommand";

		public RunCmdProvider(string command)
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