using ConDep.Dsl.Core;
using Microsoft.Web.Deployment;
using DeploymentProviderOptions = Microsoft.Web.Deployment.DeploymentProviderOptions;

namespace ConDep.Dsl
{
	public class CertficiateDeploymentProvider : WebDeployProviderBase
	{
		private const string NAME = "cert";

		public CertficiateDeploymentProvider(string thumbprint)
		{
			SourcePath = thumbprint;
		}

		public override string Name
		{
			get { return NAME; }
		}

		public override DeploymentProviderOptions GetWebDeployDestinationObject()
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