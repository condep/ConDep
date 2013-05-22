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
    }
}