using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
    public abstract class InfrastructureArtifact
    {
        protected abstract void Configure(IOfferInfrastructure require);
    }
}