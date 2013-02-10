namespace ConDep.Dsl
{
    public interface IOfferRunCmdOptions
    {
        IOfferRunCmdOptions WaitIntervalInSeconds(int waitInterval);
        IOfferRunCmdOptions RetryAttempts(int retryAttempts);
        IOfferRunCmdOptions ContinueOnError(bool continueOnError);
    }
}