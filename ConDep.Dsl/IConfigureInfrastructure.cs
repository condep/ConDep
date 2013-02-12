using ConDep.Dsl.Operations;

namespace ConDep.Dsl
{
    /// <summary>
    /// Expose functionality for custom infrastructure operations to be added to ConDep's execution sequence.
    /// </summary>
    public interface IConfigureInfrastructure
    {
        void AddOperation(RemoteCompositeInfrastructureOperation operation);
    }
}