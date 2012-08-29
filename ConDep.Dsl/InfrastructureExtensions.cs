using System;

namespace ConDep.Dsl.Core
{
    public static class InfrastructureExtensions
    {
        public static void Iis(this ProvideForInfrastructure infrastructureOptions, Action<ProvideForInfrastructureIis> iisOptions)
        {
            //var options = (InfrastructureProviderOptions) infrastructureOptions;
            //iisOptions(new InfrastructureIisOptions(((IProvideOptions)infrastructureOptions).WebDeploySetup));
            var infraIis = new ProvideForInfrastructureIis();
            ((IProvideOptions)infraIis).AddProviderAction = ((IProvideOptions)infrastructureOptions).WebDeploySetup.ConfigureProvider;
            iisOptions(infraIis);
        }
    }

    public class ProvideForInfrastructureIis : IProvideOptions
    {
        ISetupWebDeploy IProvideOptions.WebDeploySetup { get; set; }
        Action<IProvide> IProvideOptions.AddProviderAction { get; set; }
    }
}