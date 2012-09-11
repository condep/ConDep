using System;
using ConDep.Dsl;

namespace ConDep.Dsl
{
    public class ProvideForDeployment : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
        IProvideConditions IProvideOptions.Condition { get; set; }
    }
}