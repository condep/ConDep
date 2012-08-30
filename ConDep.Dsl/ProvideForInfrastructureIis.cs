using System;

namespace ConDep.Dsl
{
    public class ProvideForInfrastructureIis : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
    }
}