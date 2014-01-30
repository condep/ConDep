namespace ConDep.Server.Domain.Deployment.Model
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