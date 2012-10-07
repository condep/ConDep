using System;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface IProvideOptions
    {
        ISetupWebDeploy WebDeploySetup { get; set; }
        Action<IProvide> AddProviderAction { get; set; }
        IProvideConditions Condition { get; set; }
        CustomProviderConfig CustomConfig { get; set; }
    }
}