using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class CopyFileProvider : Provider
	{
		private const string NAME = "filePath";

		public CopyFileProvider(string sourcePath)
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
	}
}