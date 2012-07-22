namespace ConDep.Dsl.Core
{
    public class InfrastructureProviderOptions : ProviderOptions, IProvideForInfrastructure
    {
        private readonly WebDeployDefinition _webDeployDefinition;
        private readonly DeploymentServer _server;
        private InfrastructureIisOptions _iisOptions;
        private InfrastructureWindowsOptions _windowsOptions;

        public InfrastructureProviderOptions(WebDeployDefinition webDeployDefinition, DeploymentServer server) : base(webDeployDefinition.Providers)
        {
            _webDeployDefinition = webDeployDefinition;
            _server = server;
        }

        public InfrastructureIisOptions IIS
        {
            get { return _iisOptions ?? (_iisOptions = new InfrastructureIisOptions(_webDeployDefinition, _server)); }
        }

        public InfrastructureWindowsOptions Windows
        {
            get { return _windowsOptions ?? (_windowsOptions = new InfrastructureWindowsOptions(_webDeployDefinition)); }
        }
    }
}