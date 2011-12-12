using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
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