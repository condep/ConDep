using System;

namespace ConDep.Dsl.Core
{
    public class ProvideForDeploymentIis : IProvideOptions
    {
        public ISetupWebDeploy WebDeploySetup { get; set; }
        public Action<IProvide> AddProviderAction { get; set; }
    }
}