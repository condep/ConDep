using StructureMap;

namespace ConDep.Dsl.Core
{
    public class IoCBootstrapper : IBootstrapper
    {
        public void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.AddRegistry(new ProvideForRegistry());
                                         });
        }

        public static void Bootstrap()
        {
            new IoCBootstrapper().BootstrapStructureMap();
        }
    }
}