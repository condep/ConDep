using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class WebAppDeploymentExtension
	{
		public static void WebApp(this ProvideForDeploymentIis providerOptions, string sourceDir, string webAppName, string destinationWebSiteName)
		{
            var webAppProvider = new WebAppDeploymentProvider(sourceDir, webAppName, destinationWebSiteName);
		    ((IProvideOptions) providerOptions).AddProviderAction(webAppProvider);
		}

        public static void WebApp(this ProvideForDeploymentIis providerOptions, string sourceWebSiteName, string sourceWebAppName, string destinationWebSiteName, string destinationWebAppName)
        {
            var webAppProvider = new WebAppDeploymentProvider(sourceWebSiteName, sourceWebAppName, destinationWebSiteName, destinationWebAppName);
            ((IProvideOptions)providerOptions).AddProviderAction(webAppProvider);
        }
    }
}