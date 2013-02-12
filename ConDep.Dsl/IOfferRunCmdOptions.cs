namespace ConDep.Dsl
{
    public interface IOfferRunCmdOptions
    {
        /// <summary>
        /// How long ConDep will wait for command to finish executing before giving up and issue a timeout exception. Actual wait time will be this wait interval * RetryAttempts (see RetryAttempts).
        /// </summary>
        /// <param name="waitInterval"></param>
        /// <returns></returns>
        IOfferRunCmdOptions WaitIntervalInSeconds(int waitInterval);

        /// <summary>
        /// How many times ConDep will retry operation. Consider this together with WaitIntervalInSeconds
        /// </summary>
        /// <param name="retryAttempts"></param>
        /// <returns></returns>
        IOfferRunCmdOptions RetryAttempts(int retryAttempts);

        /// <summary>
        /// If true, will continue execution even if an error occur during operation execution
        /// </summary>
        /// <param name="continueOnError"></param>
        /// <returns></returns>
        IOfferRunCmdOptions ContinueOnError(bool continueOnError);
    }
}