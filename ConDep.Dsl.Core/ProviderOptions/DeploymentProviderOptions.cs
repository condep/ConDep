namespace ConDep.Dsl.Core
{
    public class DeploymentProviderOptions : ProviderOptions, IProvideForDeployment
    {
        private readonly WebDeployDefinition _webDeployDefinition;
        private DeploymentIisOptions _iisOptions;
        private WindowsOptions _windowsOptions;

        public DeploymentProviderOptions(WebDeployDefinition webDeployDefinition) : base(webDeployDefinition.Providers)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public DeploymentIisOptions IIS
        {
            get { return _iisOptions ?? (_iisOptions = new DeploymentIisOptions(_webDeployDefinition)); }
        }

        public WindowsOptions Windows
        {
            get { return _windowsOptions ?? (_windowsOptions = new WindowsOptions(_webDeployDefinition)); }
        }
    }
}