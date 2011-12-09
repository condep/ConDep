using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class CopyDirProvider : Provider
	{
		private const string NAME = "dirPath";

		public CopyDirProvider(string sourcePath)
		{
			SourcePath = sourcePath;
			Name = NAME;
		}

		public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
			return new DeploymentProviderOptions(Name) { Path = DestinationPath };
		}

		public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return DeploymentManager.CreateObject(Name, SourcePath, sourceBaseOptions);
		}

		public override bool IsValid()
		{
			return !(string.IsNullOrWhiteSpace(SourcePath) && string.IsNullOrWhiteSpace(DestinationPath));
		}
	}
}