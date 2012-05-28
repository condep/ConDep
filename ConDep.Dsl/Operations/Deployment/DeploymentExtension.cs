using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
	public static class DeploymentExtension
	{
        //public static void Sync(this SetupOptions setupOptions, Action<SyncOptions> action)
        //{
        //    var webDeployDefinition = new WebDeployDefinition();

        //    var webDeployOperation = new DeploymentOperation(webDeployDefinition);
        //    setupOptions.AddOperation(webDeployOperation);

        //    action(new SyncOptions(webDeployDefinition));
        //}

        public static void Deployment(this DeploymentOptions deploymentOptions, string destinationServerName, Action<IProvideForDeployment> serverSetup)
        {
            var webDeployDefinition = new WebDeployDefinition();
            webDeployDefinition.Destination.ComputerName = destinationServerName;

            //ToDo: Add overload for username and password

            var webDeployOperation = new DeploymentOperation(webDeployDefinition);
            deploymentOptions.AddOperation(webDeployOperation);

            serverSetup(new DeploymentProviderOptions(webDeployDefinition));
        }
	}
}