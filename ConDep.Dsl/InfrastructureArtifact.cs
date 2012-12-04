namespace ConDep.Dsl
{
    public abstract class InfrastructureArtifact
    {
        protected abstract void Configure(IConfigureInfrastructure require);
    }
}