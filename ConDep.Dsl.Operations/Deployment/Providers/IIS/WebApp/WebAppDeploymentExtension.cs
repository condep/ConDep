using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class WebAppDeploymentExtension
	{
		public static void WebApp(this IProvideForExistingIisServer providerCollection, string sourceDir, string webAppName, string destinationWebSiteName)
		{
		    AddProvider(sourceDir, webAppName, destinationWebSiteName, providerCollection);
		}

        public static void WebApp(this IProvideForExistingIisServer providerCollection, string sourceWebSiteName, string sourceWebAppName, string destinationWebSiteName, string destinationWebAppName)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceWebSiteName, sourceWebAppName, destinationWebSiteName, destinationWebAppName);
            providerCollection.AddProvider(webAppProvider);
        }

        private static void AddProvider(string sourceDir, string webAppName, string destinationWebSiteName, IProviderForAll providerCollection)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, destinationWebSiteName);
            providerCollection.AddProvider(webAppProvider);
        }
    }
}