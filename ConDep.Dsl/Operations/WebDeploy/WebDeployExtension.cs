using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class WebDeployExtension
	{
		public static void WebDeploy(this SetupOptions setupOptions, Action<SyncOptions> action)
		{
			var webDeployDefinition = new WebDeployDefinition();

			var webDeployOperation = new WebDeployOperation(webDeployDefinition);
			setupOptions.AddOperation(webDeployOperation);

			action(new SyncOptions(webDeployDefinition));
		}
	}
}