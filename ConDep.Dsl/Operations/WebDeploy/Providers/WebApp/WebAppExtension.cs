using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class WebAppExtension
	{
		public static void WebApp(this ProviderCollection providerCollection, string sourceDir, string webAppName, string destinationWebSiteName)
		{
			var webAppProvider = new WebAppProvider(sourceDir, webAppName, destinationWebSiteName);
			providerCollection.AddProvider(webAppProvider);
		}

	}
}