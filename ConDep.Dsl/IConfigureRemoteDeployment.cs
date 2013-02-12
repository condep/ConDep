using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl
{
    /// <summary>
    /// Expose functionality for custom remote operations to be added to ConDep's execution sequence.
    /// </summary>
    public interface IConfigureRemoteDeployment
    {
        void AddOperation(RemoteCompositeOperation operation);
        void AddOperation(WebDeployProviderBase provider);
    }
}