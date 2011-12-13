using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
	public abstract class Provider : IProvide, IWebDeployModel
	{
		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public string Name { get; set; }

		public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
		public abstract bool IsValid(Notification notification);
	}
}