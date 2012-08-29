using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl
{
    public class ProvideForInfrastructure : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
    }

    public class ProvideForDeployment : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
    }
}