using ConDep.Dsl.Operations.Application.Local;

namespace ConDep.Dsl
{
    public interface IConfigureLocalOperations
    {
        void AddOperation(LocalOperation operation);
    }
}