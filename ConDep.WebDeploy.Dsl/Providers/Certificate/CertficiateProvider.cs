using ConDep.WebDeploy.Dsl.SemanticModel;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl
{
	public class CertficiateProvider : Provider
	{
		private const string NAME = "cert";

		public CertficiateProvider(string thumbprint)
		{
			SourcePath = thumbprint;
			Name = NAME;
		}

		public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
			return new DeploymentProviderOptions(DeploymentWellKnownProvider.Auto);
		}

		public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			var obj = DeploymentManager.CreateObject(Name, SourcePath, sourceBaseOptions);
			return obj;
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(SourcePath);
		}
	}
}