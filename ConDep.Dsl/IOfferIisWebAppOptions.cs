namespace ConDep.Dsl
{
    public interface IOfferIisWebAppOptions
    {
        /// <summary>
        /// The physical path of the Web Application. If not set, a sub directory with the Web Application name under the Web Site will be used by default.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IOfferIisWebAppOptions PhysicalPath(string path);

        /// <summary>
        /// Associate Application Pool with Web Application. If not set, the Application Pool for the Web Site will be used.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IOfferIisWebAppOptions AppPool(string name);
    }
}