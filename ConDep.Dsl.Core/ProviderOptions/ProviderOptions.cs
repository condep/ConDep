using System.Collections.Generic;

namespace ConDep.Dsl.Core
{
    //Todo: Is this class needed at all? Could this be integrated into each provider?
    public class ProviderOptions : IProvideForExistingIisServer, IProvideForInfrastructureIis
    {
        private readonly List<IProvide> _providers;
        private readonly DeploymentServer _server;

        public ProviderOptions(List<IProvide> providers)
        {
            _providers = providers;
        }

        public ProviderOptions(List<IProvide> providers, DeploymentServer server)
        {
            _providers = providers;
            _server = server;
        }

        public void AddProvider(IProvide provider)
        {
            _providers.Add(provider);

            if (provider is WebDeployCompositeProvider)
            {
                ((WebDeployCompositeProvider)provider).Configure(_server);
            }
        }
    }
}