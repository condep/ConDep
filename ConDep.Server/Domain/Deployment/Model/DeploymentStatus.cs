namespace ConDep.Server.Model.DeploymentAggregate
{
    public enum DeploymentStatus
    {
        AwaitingExecution,
        InProgress,
        Success,
        Failed,
        Cancelled
    }
}