namespace ConDep.Dsl
{
    public interface IOfferWindowsServiceFailureOptions
    {
        /// <summary>
        /// Provide possible actions that should be performed after first failure of a Windows Service.
        /// </summary>
        IOfferWindowsServiceFailureActions FirstFailure { get; }

        /// <summary>
        /// Provide possible actions that should be performed after second failure of a Windows Service.
        /// </summary>
        IOfferWindowsServiceFailureActions SecondFailure { get; }

        /// <summary>
        /// Provide possible actions that should be performed after every subsequent Windows Service failure (after first and second).
        /// </summary>
        IOfferWindowsServiceFailureActions SubsequentFailures { get; }
    }
}