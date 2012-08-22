using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetupExtensions
	{
        public static void Deployment(this IProvideForSetup conDepSetup, Action<IProvideForDeployment> deployment)
        {
            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                var setup = (ConDepSetup) conDepSetup;
                setup.WebDeploySetup.ConfigureServer(deploymentServer, setup);

                deployment(new DeploymentProviderOptions(setup.WebDeploySetup));
            }
        }

        public static void Infrastructure(this IProvideForSetup conDepSetup, Action<IProvideForInfrastructure> infrastructure)
        {
            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                var setup = (ConDepSetup)conDepSetup;
                setup.WebDeploySetup.ConfigureServer(deploymentServer, setup);

                infrastructure(new InfrastructureProviderOptions(setup.WebDeploySetup));
            }
        }
	}

    public static class InfrastructureExtensions
    {
        public static void Iis(this IProvideForInfrastructure infrastructureOptions, Action<IProvideForInfrastructureIis> iisOptions)
        {
            var options = (InfrastructureProviderOptions) infrastructureOptions;
            iisOptions(new InfrastructureIisOptions(options.WebDeploySetup));
        }
    }

    public static class DeploymentExtensions
    {
        public static void Iis(this IProvideForDeployment deploymentOptions, Action<IProvideForDeploymentIis> iisOptions)
        {
            var options = (DeploymentProviderOptions)deploymentOptions;
            iisOptions(new DeploymentIisOptions(options.WebDeploySetup));
        }
    }

    public static class DeploymentIisExtensions
    {
        //Todo: Add provider for syncing from existing iis server
        public static void SyncFromExistingServer(this IProvideForDeploymentIis iis, string iisServer, Action<IProvideForDeploymentExistingIis> sync)
        {
            var options = (DeploymentIisOptions) iis;
            options.WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.ComputerName = iisServer;
            sync(new DeploymentExistingIisOptions(options.WebDeploySetup));
        }

        public static void SyncFromExistingServer(this IProvideForDeploymentIis iis, string iisServer, string serverUserName, string serverPassword, Action<IProvideForDeploymentExistingIis> sync)
        {
            var options = (DeploymentIisOptions)iis;
            options.WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.Credentials.UserName = serverUserName;
            options.WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.Credentials.Password = serverPassword;

            SyncFromExistingServer(iis, iisServer, sync);
        }
    }
}
