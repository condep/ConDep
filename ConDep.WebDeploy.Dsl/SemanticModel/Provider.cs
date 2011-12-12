using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public abstract class Provider : IWebDeployModel
	{
		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public string Name { get; set; }

		public abstract DeploymentProviderOptions GetWebDeployDestinationProviderOptions();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
		public abstract bool IsValid(Notification notification);
	}
}