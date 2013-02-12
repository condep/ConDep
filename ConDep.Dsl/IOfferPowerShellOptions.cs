namespace ConDep.Dsl
{
    public interface IOfferPowerShellOptions
    {
        /// <summary>
        /// If used will make sure ConDep's remote .NET library are available on remote servers. For now this is only used internally.
        /// </summary>
        /// <returns></returns>
        IOfferPowerShellOptions RequireRemoteLib();

        /// <summary>
        /// If true, will continue execution even if an error occur during operation execution
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IOfferPowerShellOptions ContinueOnError(bool value);

        /// <summary>
        /// How long ConDep will wait for command to finish executing before giving up and issue a timeout exception. Actual wait time will be this wait interval * RetryAttempts (see RetryAttempts).
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        IOfferPowerShellOptions WaitIntervalInSeconds(int seconds);

        /// <summary>
        /// How many times ConDep will retry operation. Consider this together with WaitIntervalInSeconds
        /// </summary>
        /// <param name="attempts"></param>
        /// <returns></returns>
        IOfferPowerShellOptions RetryAttempts(int attempts);
    }
}