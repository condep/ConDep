using System;
using ConDep.Dsl.FluentWebDeploy.Builders;
using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy;
using ConDep.Dsl.FluentWebDeploy.Operations.WebDeploy.Model;

namespace ConDep.Dsl.FluentWebDeploy
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