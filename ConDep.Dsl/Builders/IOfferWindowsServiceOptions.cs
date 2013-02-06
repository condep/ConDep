namespace ConDep.Dsl.Builders
{
    public interface IOfferWindowsServiceOptions
    {
        /// <summary>
        /// Use this if a <seealso cref="System.ServiceProcess.ServiceInstaller"/> class is used for your Windows Service. Specify the parameters needed to install the serivce using the installer.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        WindowsServiceOptions UseServiceInstaller(string parameters);
        WindowsServiceOptions UserName(string username);
        WindowsServiceOptions Password(string password);
        WindowsServiceOptions DisplayName(string displayName);
        WindowsServiceOptions Description(string description);
        WindowsServiceOptions ServiceGroup(string group);

        /// <summary>
        /// Any parameters this service should be executed with
        /// </summary>
        WindowsServiceOptions ExeParams(string parameters);

        /// <summary>
        /// Interval in seconds with no failures after which the failure count is reset to 0
        /// </summary>
        /// <param name="interval">Interval in seconds</param>
        /// <returns></returns>
        WindowsServiceOptions ServiceFailureResetInterval(int interval);

        /// <summary>
        /// Delay in millisecond before the service gets restarted after failure
        /// </summary>
        /// <param name="delay">Delay in milliseconds</param>
        /// <returns></returns>
        WindowsServiceOptions ServiceRestartDelay(int delay);

        WindowsServiceOptions IgnoreFailureOnServiceStartStop(bool value);
    }
}