using System;

namespace ConDep.Dsl.Core
{
    public static class DeploymentExtensions
    {
        public static void Iis(this ProvideForDeployment deploymentOptions, Action<IProvideForDeploymentIis> iisOptions)
        {
            //var options = (DeploymentProviderOptions)deploymentOptions;
            iisOptions(new DeploymentIisOptions(((IProvideOptions) deploymentOptions).WebDeploySetup));
        }
    }
}