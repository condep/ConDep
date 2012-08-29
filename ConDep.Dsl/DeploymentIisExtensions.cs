using System;

namespace ConDep.Dsl.Core
{
    public static class DeploymentIisExtensions
    {
        //Todo: Add provider for syncing from existing iis server
        public static void SyncFromExistingServer(this ProvideForDeploymentIis iis, string iisServer, Action<ProvideForDeploymentExistingIis> sync)
        {
            //todo: figure out how to set computer name
            //WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.ComputerName = iisServer;
            sync(new ProvideForDeploymentExistingIis());
        }

        public static void SyncFromExistingServer(this ProvideForDeploymentIis iis, string iisServer, string serverUserName, string serverPassword, Action<ProvideForDeploymentExistingIis> sync)
        {
            //options.WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.Credentials.UserName = serverUserName;
            //options.WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.Credentials.Password = serverPassword;

            //todo: figure out how to set user
            //WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.Credentials.UserName = serverUserName;
            //WebDeploySetup.ActiveWebDeployServerDefinition.WebDeploySource.Credentials.Password = serverPassword;

            SyncFromExistingServer(iis, iisServer, sync);
        }
    }
}