using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class ApplicationRequestRoutingExtension
	{
		public static void ApplicationRequestRouting(this SetupBuilder setupBuilder, string webServerName, Action<ApplicationRequestRoutingOptions> options)
		{
			var arrOperation = new ApplicationReqeustRoutingOperation(webServerName);
			setupBuilder.AddOperation(arrOperation);
			options(new ApplicationRequestRoutingOptions(arrOperation));
		}
	}
}