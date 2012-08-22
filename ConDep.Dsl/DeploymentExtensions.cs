using System;

namespace ConDep.Dsl.Core
{
    public static class DeploymentExtensions
    {
        public static void Iis(this IProvideForDeployment deploymentOptions, Action<IProvideForDeploymentIis> iisOptions)
        {
            var options = (DeploymentProviderOptions)deploymentOptions;
            iisOptions(new DeploymentIisOptions(options.WebDeploySetup));
        }
    }
}