using ConDep.Dsl.Operations.Infrastructure.IIS;

namespace ConDep.Dsl
{
    public interface IOfferIisAppPoolOptions
    {
        /// <summary>
        /// Sets the .NET framework version for the application pool
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions NetFrameworkVersion(NetFrameworkVersion version);

        /// <summary>
        /// Sets the .NET framework version for the application pool
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions NetFrameworkVersion(string version);

        /// <summary>
        /// Specifies what type of managed pipeline to use.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions ManagedPipeline(ManagedPipeline pipeline);

        /// <summary>
        /// Configures the application pool to run as a built-in account or custom account. 
        /// Valid values for built-in accounts are: LocalService, LocalSystem or NetworkService. 
        /// For custom account, provide a local or domain user, together with the password 
        /// in IdentityPassword.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions IdentityUsername(string userName);

        /// <summary>
        /// Password for custom account defined in IdentityUsername
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions IdentityPassword(string password);

        /// <summary>
        /// If your server runs in 64bit, but you have a web application that only supports 32bit, 
        /// use this to enable 32bit.
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions Enable32Bit(bool enable);

        /// <summary>
        /// Amount of time in minutes a worker process will remain idle before it shuts down. 
        /// A worker process is idle if not processing or receiving requests. Default is 20.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions IdleTimeoutInMinutes(int minutes);

        /// <summary>
        /// Specified weather IIS loads the user profile for the application pool identity.
        /// </summary>
        /// <param name="load"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions LoadUserProfile(bool load);

        /// <summary>
        /// Period of time after which an application pool will recycle. Value 0 disables recycling. 
        /// 1740 (29 hours) is default.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        IOfferIisAppPoolOptions RecycleTimeInMinutes(int minutes);
    }
}