using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;

namespace ConDep.Dsl
{
    public abstract class InfrastructureArtifact
    {
        public abstract void Configure(IOfferInfrastructure require);

        public void Execute(ServerConfig server)
        {
            
        }
    }
}