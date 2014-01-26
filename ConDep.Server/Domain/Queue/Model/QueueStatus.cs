namespace ConDep.Server.Model.QueueAggregate
{
    public enum QueueStatus
    {
        Waiting,
        ReadyForDeployment,
        DeploymentInProgress,
        Processed
    }
}