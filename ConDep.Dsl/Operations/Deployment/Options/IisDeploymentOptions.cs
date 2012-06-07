using System;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl.Builders
{
    public class IisDeploymentOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public IisDeploymentOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public void SyncFromExistingServer(string iisServer, Action<IProvideForExistingIisServer> sync)
        {
            _webDeployDefinition.Source.ComputerName = iisServer;
            sync(new ProviderOptions(_webDeployDefinition.Providers));
        }

        public void SyncFromExistingServer(string iisServer, string serverUserName, string serverPassword, Action<IProvideForExistingIisServer> sync)
        {
            _webDeployDefinition.Destination.Credentials.UserName = serverUserName;
            _webDeployDefinition.Destination.Credentials.Password = serverPassword;

            SyncFromExistingServer(iisServer, sync);
        }

    }
}