using System;

namespace ConDep.Dsl.Core
{
    public class DeploymentIisOptions : IProvideForDeploymentIis
    {
        private readonly ISetupWebDeploy _webDeploySetup;

        public DeploymentIisOptions(ISetupWebDeploy webDeploySetup)
        {
            _webDeploySetup = webDeploySetup;
        }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }
    }
}