using System;

namespace ConDep.Dsl.Core
{
    public class InfrastructureIisOptions : IProvideForInfrastructureIis
    {
        private readonly ISetupWebDeploy _webDeploySetup;

        public InfrastructureIisOptions(ISetupWebDeploy webDeploySetup)
        {
            _webDeploySetup = webDeploySetup;
        }

        public ISetupWebDeploy WebDeploySetup
        {
            get { return _webDeploySetup; }
        }
    }

}