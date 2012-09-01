using System;
using ConDep.Dsl;
using ConDep.Dsl.WebDeployProviders.Deployment.General;

namespace ConDep.Dsl
{
	public static class GeneralProviderExtension
	{
        public static void DefineCustom(this ProvideForDeploymentIis iisOptions, string providername, string sourcepath, string destinationpath)
        {
			var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
            ((IProvideOptions)iisOptions).AddProviderAction(provider);
        }

        public static void DefineCustom(this ProvideForDeploymentIis iisOptions, string providername, string sourcepath, string destinationpath, Action<GeneralProviderOptions> generalOptions)
		{
            var provider = new GeneralProvider(providername) { SourcePath = sourcepath, DestinationPath = destinationpath };
			var generalProviderOptions = new GeneralProviderOptions(provider);

			generalOptions(generalProviderOptions);
            ((IProvideOptions)iisOptions).AddProviderAction(provider);
        }

	}
}