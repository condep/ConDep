using StructureMap;

namespace ConDep.Dsl.Core
{
    public class IoCBootstrapper : IBootstrapper
    {
        public void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.AddRegistry(new IoCProviderOptionsRegistry());
                                         });
        }

        public static void Bootstrap()
        {
            new IoCBootstrapper().BootstrapStructureMap();
        }
    }
}