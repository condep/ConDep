using System;

namespace ConDep.Dsl.Core
{
    public interface IProvideOptions
    {
        ISetupWebDeploy WebDeploySetup { get; set; }
        Action<IProvide> AddProviderAction { get; set; }
    }

    //public class ProviderOptions<T> : IProvideOptions where T : IProvideOptions
    //{
    //}

    //public class DeploymentProviderOptions : IProvideForDeployment
    //{
    //    public DeploymentProviderOptions(ISetupWebDeploy webDeploySetup)
    //    {
    //        WebDeploySetup = webDeploySetup;
    //    }

    //    public ISetupWebDeploy WebDeploySetup { get; set; }
    //    public IProvide Provider { get; set; }
    //}
}