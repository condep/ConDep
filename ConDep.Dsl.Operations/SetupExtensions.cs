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
                var webDeployDefinition = ConfigureWebDeploy(deploymentServer, (ConDepSetup)conDepSetup);
                deployment(new DeploymentProviderOptions(webDeployDefinition));
            }
        }

        public static void Infrastructure(this ISetupCondep conDepSetup, Action<IProvideForInfrastructure> infrastructure)
        {
            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                var webDeployDefinition = ConfigureWebDeploy(deploymentServer, (ConDepSetup)conDepSetup);
                infrastructure(new InfrastructureProviderOptions(webDeployDefinition, deploymentServer));
            }
        }

	    private static WebDeployDefinition ConfigureWebDeploy(DeploymentServer deploymentServer, ConDepSetup setupOptions)
	    {
	        var webDeployDefinition = new WebDeployDefinition();
	        webDeployDefinition.WebDeployDestination.ComputerName = deploymentServer.ServerName;
	        webDeployDefinition.WebDeploySource.LocalHost = true;

	        if(ConDepConfigurator.EnvSettings.DeploymentUser.IsDefined)
	        {
	            webDeployDefinition.WebDeployDestination.Credentials.UserName = ConDepConfigurator.EnvSettings.DeploymentUser.UserName;
	            webDeployDefinition.WebDeployDestination.Credentials.Password = ConDepConfigurator.EnvSettings.DeploymentUser.Password;

	            webDeployDefinition.WebDeploySource.Credentials.UserName = ConDepConfigurator.EnvSettings.DeploymentUser.UserName;
	            webDeployDefinition.WebDeploySource.Credentials.Password = ConDepConfigurator.EnvSettings.DeploymentUser.Password;
	        }

	        var webDeployOperation = new WebDeployOperation(webDeployDefinition);
	        setupOptions.AddOperation(webDeployOperation);
	        return webDeployDefinition;
	    }
	}
}