namespace ConDep.Dsl
{
    public interface IOfferRemoteOperations
    {
        /// <summary>
        /// Provide operations for remote deployment.
        /// </summary>
        IOfferRemoteDeployment Deploy { get; }

        /// <summary>
        /// Provide operations for remote execution.
        /// </summary>
        IOfferRemoteExecution ExecuteRemote { get; }
    }
}