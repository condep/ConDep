using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetupExtensions
	{
        public static void Deployment(this ISetupCondep conDepSetup, Action<IProvideForDeployment> deployment)
        {
            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                var webDeployDefinition = WebDeployDefinition.CreateOrGetForServer(ConDepConfigurator.EnvSettings, deploymentServer);
                
                //Todo: Check if this should be done before or after calling the action
                var webDeployOperation = new WebDeployOperation(webDeployDefinition);
                ((ConDepSetup)conDepSetup).AddOperation(webDeployOperation);

                deployment(new DeploymentProviderOptions(webDeployDefinition));
            }
        }

        public static void Infrastructure(this ISetupCondep conDepSetup, Action<IProvideForInfrastructure> infrastructure)
        {
            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                var webDeployDefinition = WebDeployDefinition.CreateOrGetForServer(ConDepConfigurator.EnvSettings, deploymentServer);
                
                //Todo: Check if this should be done before or after calling the action
                var webDeployOperation = new WebDeployOperation(webDeployDefinition);
                ((ConDepSetup)conDepSetup).AddOperation(webDeployOperation);

                infrastructure(new InfrastructureProviderOptions(webDeployDefinition, deploymentServer));
            }
        }
	}
}