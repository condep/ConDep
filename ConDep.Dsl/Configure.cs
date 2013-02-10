namespace ConDep.Dsl
{
    public static class Configure
    {
        public static IConfigureLocalOperations LocalOperations { get; internal set; }

        public static IConfigureRemoteDeployment DeploymentOperations { get; internal set; }

        public static IConfigureRemoteExecution ExecutionOperations { get; internal set; }

        public static IConfigureInfrastructure InfrastructureOperations { get; internal set; }
    }
}