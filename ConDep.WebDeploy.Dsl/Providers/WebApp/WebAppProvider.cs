using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class WebAppProvider : Provider
	{
		private const string NAME = "iisApp";

		public WebAppProvider(string sourcePath)
		{
			Name = NAME;
			SourcePath = sourcePath;
		}

		public string DestinationWebSite { get; set; }
		public string DestinationAppName { get; set; }

		public override string DestinationPath
		{
			get
			{
				return DestinationWebSite + "/" + DestinationAppName;
			}
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
			throw new System.NotImplementedException();
		}
	}
}