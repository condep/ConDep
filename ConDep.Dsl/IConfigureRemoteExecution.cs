using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl
{
    public interface IConfigureRemoteExecution
    {
        void AddOperation(RemoteCompositeOperation operation);
        void AddOperation(WebDeployProviderBase provider);
    }
}