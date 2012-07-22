using System;

namespace ConDep.Dsl.Core
{
    public class InfrastructureIisOptions
    {
        private WebDeployDefinition _webDeployDefinition;
        private readonly DeploymentServer _server;

        public InfrastructureIisOptions(WebDeployDefinition webDeployDefinition, DeploymentServer server)
        {
            _webDeployDefinition = webDeployDefinition;
            _server = server;
        }

        public void Define(Action<IProvideForInfrastructureIis> iisDefinition)
        {
            iisDefinition(new ProviderOptions(_webDeployDefinition.Providers, _server));
        }
    }
}