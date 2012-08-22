using System;

namespace ConDep.Dsl.Core
{
    public static class InfrastructureExtensions
    {
        public static void Iis(this IProvideForInfrastructure infrastructureOptions, Action<IProvideForInfrastructureIis> iisOptions)
        {
            var options = (InfrastructureProviderOptions) infrastructureOptions;
            iisOptions(new InfrastructureIisOptions(options.WebDeploySetup));
        }
    }
}