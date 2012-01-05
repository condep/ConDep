using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
	public static class WebDeployExtension
	{
		public static void WebDeploy(this SetupBuilder setupBuilder, Action<SyncBuilder> action)
		{
			var webDeployDefinition = new WebDeployDefinition();

			var webDeployOperation = new WebDeployOperation(webDeployDefinition);
			setupBuilder.AddOperation(webDeployOperation);

			action(new SyncBuilder(webDeployDefinition));
		}
	}
}