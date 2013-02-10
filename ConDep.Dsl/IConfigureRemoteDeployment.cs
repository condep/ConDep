using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl
{
    public interface IConfigureRemoteDeployment
    {
        void AddOperation(RemoteCompositeOperation operation);
        void AddOperation(WebDeployProviderBase provider);
    }
}