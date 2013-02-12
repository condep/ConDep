using ConDep.Dsl.Config;

namespace ConDep.Dsl
{
    /// <summary>
    /// Inherit this class to configure deployment for your application
    /// </summary>
    public abstract class ApplicationArtifact
    {
        public abstract void Configure(IOfferLocalOperations onLocalMachine, ConDepConfig config);
    }
}