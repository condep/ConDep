using System;
using System.ServiceProcess;

namespace ConDep.Dsl
{
    public interface IOfferWindowsServiceOptions
    {
        IOfferWindowsServiceOptions UserName(string username);
        IOfferWindowsServiceOptions Password(string password);
        IOfferWindowsServiceOptions Description(string description);
        IOfferWindowsServiceOptions ServiceGroup(string group);
        IOfferWindowsServiceOptions ExeParams(string parameters);
        IOfferWindowsServiceOptions ServiceFailureResetInterval(int interval);
        IOfferWindowsServiceOptions ServiceRestartDelay(int delay);
        IOfferWindowsServiceOptions IgnoreFailureOnServiceStartStop(bool value);
        IOfferWindowsServiceOptions StartupType(ServiceStartMode type);
        IOfferWindowsServiceOptions DoNotStartAfterInstall();
        IOfferWindowsServiceOptions OnServiceFailure(int serviceFailureResetInterval, Action<IOfferWindowsServiceFailureOptions> options);
    }
}