namespace ConDep.Server.Domain.Queue.Model
{
    public enum QueueStatus
    {
        Waiting,
        ReadyForDeployment,
        DeploymentInProgress,
        Processed
    }
}