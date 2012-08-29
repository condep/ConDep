using System;
using StructureMap;

namespace ConDep.Dsl.Core
{
    public class IoCBootstrapper : IBootstrapper
    {
        private readonly ConDepEnvironmentSettings _envSettings;

        private IoCBootstrapper(ConDepEnvironmentSettings envSettings)
        {
            _envSettings = envSettings;
        }

        public void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.AddRegistry(new IoCProviderOptionsRegistry(_envSettings));
                                         });
        }

        public static void Bootstrap(ConDepEnvironmentSettings envSettings)
        {
            new IoCBootstrapper(envSettings).BootstrapStructureMap();
        }
    }
}