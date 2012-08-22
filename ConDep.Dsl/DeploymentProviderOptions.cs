namespace ConDep.Dsl.Core
{
    public class DeploymentProviderOptions : IProvideForDeployment
    {
        private readonly ISetupWebDeploy _webDeploySetup;

        public DeploymentProviderOptions(ISetupWebDeploy webDeploySetup)
        {
            _webDeploySetup = webDeploySetup;
        }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }
    }
}