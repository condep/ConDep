using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
	public abstract class Provider : IProvide, IWebDeployModel
	{
		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public abstract string Name { get; }
		public int WaitInterval { get; set; }

		public abstract DeploymentProviderOptions GetWebDeployDestinationObject();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
		public abstract bool IsValid(Notification notification);
	}
}