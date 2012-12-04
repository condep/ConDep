using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;

namespace ConDep.Dsl
{
    public abstract class ApplicationArtifact
    {
        public abstract void Configure(IOfferLocalOperations local, ConDepConfig config);
    }
}