namespace ConDep.Dsl.Experimental.Application.Infrastructure
{
    public abstract class InfrastructureArtifact
    {
        protected abstract void Configure(IConfigureInfrastructure require);
    }
}