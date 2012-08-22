using System;

namespace ConDep.Dsl.Core
{
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