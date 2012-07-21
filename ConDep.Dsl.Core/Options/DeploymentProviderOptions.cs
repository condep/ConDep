namespace ConDep.Dsl.Core
{
    public class DeploymentProviderOptions : ProviderOptions, IProvideForDeployment, IProvideForInfrastructure
    {
        private readonly WebDeployDefinition _webDeployDefinition;
        private IisDeploymentOptions _iisOptions;
        private WindowsOptions _windowsOptions;

        public DeploymentProviderOptions(WebDeployDefinition webDeployDefinition) : base(webDeployDefinition.Providers)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public IisDeploymentOptions IIS
        {
            get { return _iisOptions ?? (_iisOptions = new IisDeploymentOptions(_webDeployDefinition)); }
        }

        public WindowsOptions Windows
        {
            get { return _windowsOptions ?? (_windowsOptions = new WindowsOptions(_webDeployDefinition)); }
        }
    }
}