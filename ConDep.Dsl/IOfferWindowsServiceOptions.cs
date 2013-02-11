using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Deployment.WindowsService;

namespace ConDep.Dsl
{
    public interface IOfferWindowsServiceOptions
    {
        /// <summary>
        /// Use this if a <seealso cref="System.ServiceProcess.ServiceInstaller"/> class is used for your Windows Service. Specify the parameters needed to install the serivce using the installer.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        WindowsServiceOptions UseServiceInstaller(string parameters);

        /// <summary>
        /// Name of the user which the Windows Service should run as
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        WindowsServiceOptions UserName(string username);
        WindowsServiceOptions Password(string password);
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

        /// <summary>
        /// If true, will ignore errors during start or stop of the Windows Service.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        WindowsServiceOptions IgnoreFailureOnServiceStartStop(bool value);
    }
}