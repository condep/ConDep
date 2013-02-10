namespace ConDep.Dsl
{
    public interface IOfferPowerShellOptions
    {
        IOfferPowerShellOptions RequireRemoteLib();
        IOfferPowerShellOptions ContinueOnError(bool value);
        IOfferPowerShellOptions WaitIntervalInSeconds(int seconds);
        IOfferPowerShellOptions RetryAttempts(int attempts);
    }
}