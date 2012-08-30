using ConDep.Dsl.LoadBalancer;
using StructureMap.Configuration.DSL;

namespace ConDep.Dsl
{
    public class IoCProviderOptionsRegistry : Registry
    {
        public IoCProviderOptionsRegistry(ConDepEnvironmentSettings envSettings)
        {
            For<ConDepEnvironmentSettings>().Use(envSettings);

            For<ISetupWebDeploy>().Use<WebDeploySetup>();
            For<ISetupConDep>().Use<ConDepSetup>();
            For<LoadBalancerSettings>().Use(envSettings.LoadBalancer);
        }
    }
}