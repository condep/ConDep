using ConDep.Dsl.Config;

namespace ConDep.Dsl
{
    /// <summary>
    /// Inherit from this class to configure infrastructure for your servers and/or applications.
    /// </summary>
    public abstract class InfrastructureArtifact
    {
        public abstract void Configure(IOfferInfrastructure require, ConDepConfig config);

        public void Execute(ServerConfig server)
        {
            
        }
    }
}