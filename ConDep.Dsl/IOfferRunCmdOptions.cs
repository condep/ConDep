namespace ConDep.Dsl
{
    public interface IOfferRunCmdOptions
    {
        /// <summary>
        /// If true, will continue execution even if an error occur during operation execution
        /// </summary>
        /// <param name="continueOnError"></param>
        /// <returns></returns>
        IOfferRunCmdOptions ContinueOnError(bool continueOnError);
    }
}