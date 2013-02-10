using ConDep.Dsl.Operations.Application.Deployment.NServiceBus;

namespace ConDep.Dsl
{
    public interface IOfferNServiceBusOptions
    {
        IOfferNServiceBusOptions ServiceInstaller(string nServiceBusInstallerPath);
        IOfferNServiceBusOptions UserName(string username);
        IOfferNServiceBusOptions Password(string password);
        IOfferNServiceBusOptions ServiceGroup(string group);

        /// <summary>
        /// Specifies which profile NServiceBus should run under
        /// </summary>
        IOfferNServiceBusOptions Profile(string profile);

        /// <summary>
        /// Interval in seconds with no failures after which the failure count is reset to 0
        /// </summary>
        /// <param name="interval">Interval in seconds</param>
        /// <returns></returns>
        IOfferNServiceBusOptions ServiceFailureResetInterval(int interval);

        /// <summary>
        /// Delay in millisecond before the service gets restarted after failure
        /// </summary>
        /// <param name="delay">Delay in milliseconds</param>
        /// <returns></returns>
        IOfferNServiceBusOptions ServiceRestartDelay(int delay);

        IOfferNServiceBusOptions IgnoreFailureOnServiceStartStop(bool value);
    }
}