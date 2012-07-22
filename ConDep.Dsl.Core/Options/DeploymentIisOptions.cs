using System;

namespace ConDep.Dsl.Core
{
    public class DeploymentIisOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public DeploymentIisOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public void SyncFromExistingServer(string iisServer, Action<IProvideForExistingIisServer> sync)
        {
            _webDeployDefinition.WebDeploySource.ComputerName = iisServer;
            sync(new ProviderOptions(_webDeployDefinition.Providers));
        }

        public void SyncFromExistingServer(string iisServer, string serverUserName, string serverPassword, Action<IProvideForExistingIisServer> sync)
        {
            _webDeployDefinition.WebDeployDestination.Credentials.UserName = serverUserName;
            _webDeployDefinition.WebDeployDestination.Credentials.Password = serverPassword;

            SyncFromExistingServer(iisServer, sync);
        }

    }
}