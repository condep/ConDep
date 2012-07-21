using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
	public static class SetupExtensions
	{
        //public static void Sync(this SetupOptions setupOptions, Action<SyncOptions> action)
        //{
        //    var webDeployDefinition = new WebDeployDefinition();

        //    var webDeployOperation = new DeploymentOperation(webDeployDefinition);
        //    setupOptions.AddOperation(webDeployOperation);

        //    action(new SyncOptions(webDeployDefinition));
        //}

        public static void Deployment(this SetupOptions setupOptions, string destinationServerName, Action<IProvideForDeployment> serverSetup)
        {
            var webDeployDefinition = new WebDeployDefinition();
            webDeployDefinition.Destination.ComputerName = destinationServerName;

            //ToDo: Add overload for username and password

            var webDeployOperation = new DeploymentOperation(webDeployDefinition);
            setupOptions.AddOperation(webDeployOperation);

            serverSetup(new DeploymentProviderOptions(webDeployDefinition));
        }

        public static void Deployment(this SetupOptions setupOptions, Action<IProvideForDeployment> serverSetup)
        {
            var webDeployDefinition = new WebDeployDefinition();

            //ToDo: Add overload for username and password

            var webDeployOperation = new DeploymentOperation(webDeployDefinition);
            setupOptions.AddOperation(webDeployOperation);

            serverSetup(new DeploymentProviderOptions(webDeployDefinition));
        }

        public static void Infrastructure(this SetupOptions setupOptions, Action<InfrastructureProviderOptions> infrastructureOptions)
        {
            if(ConDepConfigurator.EnvSettings.LoadBalancer.IsDefined)
            {
                throw new NotImplementedException("Support for load balancer not yet implemented.");
            }

            foreach (var deploymentServer in ConDepConfigurator.EnvSettings.Servers)
            {
                var webDeployDefinition = new WebDeployDefinition();
                webDeployDefinition.Destination.ComputerName = deploymentServer.ServerName;
                webDeployDefinition.Source.LocalHost = true;

                if(ConDepConfigurator.EnvSettings.DeploymentUser.IsDefined)
                {
                    webDeployDefinition.Destination.Credentials.UserName = ConDepConfigurator.EnvSettings.DeploymentUser.UserName;
                    webDeployDefinition.Destination.Credentials.Password = ConDepConfigurator.EnvSettings.DeploymentUser.Password;

                    webDeployDefinition.Source.Credentials.UserName = ConDepConfigurator.EnvSettings.DeploymentUser.UserName;
                    webDeployDefinition.Source.Credentials.Password = ConDepConfigurator.EnvSettings.DeploymentUser.Password;
                }

                var webDeployOperation = new DeploymentOperation(webDeployDefinition);
                setupOptions.AddOperation(webDeployOperation);

                infrastructureOptions(new InfrastructureProviderOptions(webDeployDefinition, deploymentServer));
            }
        }
    }
}