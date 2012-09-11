using System;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public class ProvideForDeploymentExistingIis : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
        IProvideConditions IProvideOptions.Condition { get; set; }
    }
}