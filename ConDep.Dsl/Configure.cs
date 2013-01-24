using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl
{
    public static class Configure
    {
        public static IConfigureLocalOperations LocalOperations { get; internal set; }

        public static IConfigureRemoteDeployment DeploymentOperations { get; internal set; }

        public static IConfigureRemoteExecution ExecutionOperations { get; internal set; }

        public static IConfigureInfrastructure InfrastructureOperations { get; internal set; }
    }

    public interface IConfigureInfrastructure
    {
        void AddOperation(RemoteCompositeInfrastructureOperation operation);
    }

    public interface IConfigureRemoteExecution
    {
        void AddOperation(RemoteCompositeOperation operation);
        void AddOperation(WebDeployProviderBase provider);
    }

    public interface IConfigureRemoteDeployment
    {
        void AddOperation(RemoteCompositeOperation operation);
        void AddOperation(WebDeployProviderBase provider);
    }
}