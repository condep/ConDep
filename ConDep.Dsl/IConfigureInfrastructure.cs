using ConDep.Dsl.Operations;

namespace ConDep.Dsl
{
    public interface IConfigureInfrastructure
    {
        void AddOperation(RemoteCompositeInfrastructureOperation operation);
    }
}