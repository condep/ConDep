using System;
using ConDep.Dsl.FluentWebDeploy.Builders;

namespace ConDep.Dsl.FluentWebDeploy
{
	public static class WebAppExtension
	{
		public static void WebApp(this ProviderCollectionBuilder providerCollectionBuilder, string sourceDir, string webAppName, string destinationWebSiteName)
		{
			var webAppProvider = new WebAppProvider(sourceDir, webAppName, destinationWebSiteName);
			providerCollectionBuilder.AddProvider(webAppProvider);
		}

	}
}