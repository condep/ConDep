using ConDep.Dsl.Core;
using StructureMap.Configuration.DSL;

namespace ConDep.Dsl.Core
{
    public class ProvideForRegistry : Registry
    {
        public ProvideForRegistry()
        {
            var webDeploySetup = new WebDeploySetup(ConDepConfigurator.EnvSettings);

            For<IProvideForDeployment>().Use<DeploymentProviderOptions>();
            For<IProvideForDeploymentIis>().Use<DeploymentIisOptions>();
            For<IProvideForInfrastructure>().Use<InfrastructureProviderOptions>();
            For<IProvideForInfrastructureIis>().Use<InfrastructureIisOptions>();
            //For<IProvideForInfrastrucutreWebSite>().Use<InfrastructureWebSiteOptions>();
            For<ISetupWebDeploy>().Use(webDeploySetup);
            For<ISetupCondep>().Use<ConDepSetup>();
        }
    }
}