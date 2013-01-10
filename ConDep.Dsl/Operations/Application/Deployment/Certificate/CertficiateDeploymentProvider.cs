using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.WebDeploy;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.Operations.Application.Deployment.Certificate
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

	    public override DeploymentProviderOptions GetWebDeploySourceProviderOptions()
	    {
            return new DeploymentProviderOptions(NAME) { Path = SourcePath};
        }

	    public override DeploymentProviderOptions GetWebDeployDestinationProviderOptions()
		{
			return new DeploymentProviderOptions(DeploymentWellKnownProvider.Auto);
		}

		public override bool IsValid(Notification notification)
		{
			return !string.IsNullOrWhiteSpace(SourcePath);
		}
	}
}