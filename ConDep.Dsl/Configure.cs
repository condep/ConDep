namespace ConDep.Dsl
{
    /// <summary>
    /// Contains entry points for adding custom operations. Use this class from your extension methods that exposes your custom operations.
    /// </summary>
    public static class Configure
    {
        public static IConfigureLocalOperations LocalOperations { get; internal set; }

        public static IConfigureRemoteDeployment DeploymentOperations { get; internal set; }

        public static IConfigureRemoteExecution ExecutionOperations { get; internal set; }

        public static IConfigureInfrastructure InfrastructureOperations { get; internal set; }
    }
}