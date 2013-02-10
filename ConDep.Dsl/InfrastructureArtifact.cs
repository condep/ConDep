using ConDep.Dsl.Config;

namespace ConDep.Dsl
{
    public abstract class InfrastructureArtifact
    {
        public abstract void Configure(IOfferInfrastructure require, ConDepConfig config);

        public void Execute(ServerConfig server)
        {
            
        }
    }
}