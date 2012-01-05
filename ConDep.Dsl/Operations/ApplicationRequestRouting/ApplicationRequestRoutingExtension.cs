using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.ApplicationRequestRouting;

namespace ConDep.Dsl
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