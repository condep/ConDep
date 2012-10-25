using ConDep.Dsl.Application.Infrastructure;

namespace ConDep.Dsl.Application
{
    public abstract class ApplicationArtifact<T> where T : InfrastructureArtifact
    {
        protected abstract void Configure(IDeploy deploy, IExecute execute, IExecuteLocally locally);
    }
}