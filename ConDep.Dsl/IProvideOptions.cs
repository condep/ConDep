using System;

namespace ConDep.Dsl
{
    public interface IProvideOptions
    {
        ISetupWebDeploy WebDeploySetup { get; set; }
        Action<IProvide> AddProviderAction { get; set; }
    }
}