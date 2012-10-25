namespace ConDep.Dsl.Application.Infrastructure
{
    public abstract class InfrastructureArtifact
    {
        protected abstract void Configure(IRequireInfrastructure require);
    }
}