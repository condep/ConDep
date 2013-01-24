using ConDep.Dsl.Operations.Application.Local;

namespace ConDep.Dsl.Builders
{
    public interface IConfigureLocalOperations
    {
        void AddOperation(LocalOperation operation);
    }
}