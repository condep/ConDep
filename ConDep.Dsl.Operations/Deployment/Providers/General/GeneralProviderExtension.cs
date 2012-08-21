using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class GeneralProviderExtension
	{
        public static void DefineCustom(this IProvideForDeploymentIis iisOptions, string providername, string sourcepath, string destinationpath)
        {
            var options = (DeploymentIisOptions) iisOptions;
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			options.WebDeploySetup.ConfigureProvider(provider);
		}

        public static void DefineCustom(this IProvideForDeploymentIis iisOptions, string providername, string sourcepath, string destinationpath, Action<GeneralProviderOptions> generalOptions)
		{
            var options = (DeploymentIisOptions)iisOptions;
            var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			var generalProviderOptions = new GeneralProviderOptions(provider);

			generalOptions(generalProviderOptions);
            options.WebDeploySetup.ConfigureProvider(provider);
        }

	}
}