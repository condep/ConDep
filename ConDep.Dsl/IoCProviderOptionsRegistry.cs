using StructureMap.Configuration.DSL;

namespace ConDep.Dsl.Core
{
    public class IoCProviderOptionsRegistry : Registry
    {
        public IoCProviderOptionsRegistry()
        {
            var webDeploySetup = new WebDeploySetup(ConDepConfiguratorBase.EnvSettings);

            For<IProvideForDeployment>().Use<DeploymentProviderOptions>();
            For<IProvideForDeploymentIis>().Use<DeploymentIisOptions>();
            For<IProvideForInfrastructure>().Use<InfrastructureProviderOptions>();
            For<IProvideForInfrastructureIis>().Use<InfrastructureIisOptions>();
            For<ISetupWebDeploy>().Use(webDeploySetup);
            For<ISetupCondep>().Use<ConDepSetup>();
        }
    }
}