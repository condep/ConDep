using System;

namespace ConDep.Dsl
{
    public static class InfrastructureExtensions
    {
        public static void Iis(this ProvideForInfrastructure infrastructureOptions, Action<ProvideForInfrastructureIis> iisOptions)
        {
            var infraIis = new ProvideForInfrastructureIis();
            ((IProvideOptions)infraIis).AddProviderAction = ((IProvideOptions)infrastructureOptions).WebDeploySetup.ConfigureProvider;
            iisOptions(infraIis);
        }
    }
}