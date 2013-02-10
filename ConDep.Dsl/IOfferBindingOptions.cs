namespace ConDep.Dsl
{
    public interface IOfferBindingOptions
    {
        /// <summary>
        /// Ip to associate with the binding.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        IOfferBindingOptions Ip(string ip);

        /// <summary>
        /// A port number to associate with the binding.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IOfferBindingOptions Port(int port);

        /// <summary>
        /// A host name to associate with the binding.
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        IOfferBindingOptions HostName(string hostName);
    }
}