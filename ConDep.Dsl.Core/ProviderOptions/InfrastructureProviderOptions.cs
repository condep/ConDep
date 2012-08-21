namespace ConDep.Dsl.Core
{
    public class InfrastructureProviderOptions : IProvideForInfrastructure
    {
        private readonly ISetupWebDeploy _webDeploySetup;

        public InfrastructureProviderOptions(ISetupWebDeploy webDeploySetup)
        {
            _webDeploySetup = webDeploySetup;
        }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }
    }
}