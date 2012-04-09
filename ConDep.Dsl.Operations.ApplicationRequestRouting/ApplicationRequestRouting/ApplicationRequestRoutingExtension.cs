using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.ApplicationRequestRouting;
using ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure;

namespace ConDep.Dsl
{
	public static class ApplicationRequestRoutingExtension
	{
		public static ApplicationRequestRoutingOptions ApplicationRequestRouting(this SetupOptions setupOptions, string webServerName)//, Action<ApplicationRequestRoutingOptions> options)
		{
			var arrOperation = new ApplicationReqeustRoutingOperation(webServerName);
			setupOptions.AddOperation(arrOperation);
			return new ApplicationRequestRoutingOptions(arrOperation);
			//options(new ApplicationRequestRoutingOptions(arrOperation));
		}

		public static ApplicationRequestRoutingOptions ApplicationRequestRouting(this SetupOptions setupOptions, string webServerName, UserInfo userInfo)
		{
			var arrOperation = new ApplicationReqeustRoutingOperation(webServerName, userInfo);
			setupOptions.AddOperation(arrOperation);
			return new ApplicationRequestRoutingOptions(arrOperation);
			//options(new ApplicationRequestRoutingOptions(arrOperation));
		}
	}
}