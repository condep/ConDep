using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl
{
    /// <summary>
    /// Expose functionality for custom remote execution operations to be added to ConDep's execution sequence.
    /// </summary>
    public interface IConfigureRemoteExecution
    {
        void AddOperation(RemoteCompositeOperation operation);
        void AddOperation(WebDeployProviderBase provider);
    }
}