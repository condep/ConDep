namespace ConDep.Dsl
{
    public interface IOfferWindowsServiceFailureOptions
    {
        IOfferWindowsServiceFailureActions FirstFailure { get; }
        IOfferWindowsServiceFailureActions SecondFailure { get; }
        IOfferWindowsServiceFailureActions SubsequentFailures { get; }
    }
}