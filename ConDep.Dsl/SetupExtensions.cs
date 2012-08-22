using System;

namespace ConDep.Dsl.Core
{
	public static class SetupExtensions
	{
        public static void Deployment(this IProvideForSetup conDepSetup, Action<IProvideForDeployment> deployment)
        {
            foreach (var deploymentServer in ConDepConfiguratorBase.EnvSettings.Servers)
            {
                var setup = (ConDepSetup) conDepSetup;
                setup.WebDeploySetup.ConfigureServer(deploymentServer, setup);

                deployment(new DeploymentProviderOptions(setup.WebDeploySetup));
            }
        }

        public static void Infrastructure(this IProvideForSetup conDepSetup, Action<IProvideForInfrastructure> infrastructure)
        {
            foreach (var deploymentServer in ConDepConfiguratorBase.EnvSettings.Servers)
            {
                var setup = (ConDepSetup)conDepSetup;
                setup.WebDeploySetup.ConfigureServer(deploymentServer, setup);

                infrastructure(new InfrastructureProviderOptions(setup.WebDeploySetup));
            }
        }
	}
}
