using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl
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