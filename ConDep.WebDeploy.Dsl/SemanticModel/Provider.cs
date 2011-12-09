using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Deployment;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public abstract class Provider
	{
		public string SourcePath { get; set; }
		public virtual string DestinationPath { get; set; }
		public string Name { get; set; }

		public abstract DeploymentProviderOptions GetWebDeployDestinationProviderOptions();
		public abstract DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions);
		public abstract bool IsValid();
	}

	public abstract class CustomProvider 
	{
		protected abstract IEnumerable<Provider> ChildProviders { get; }

		public IEnumerable<DeploymentProviderOptions> GetWebDeployDestinationProviderOptions()
		{
			return ChildProviders.Select(provider => provider.GetWebDeployDestinationProviderOptions());
		}

		public IEnumerable<DeploymentObject> GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
		{
			return ChildProviders.Select(provider => provider.GetWebDeploySourceObject(sourceBaseOptions));
		}
	}
}