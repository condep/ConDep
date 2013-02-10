namespace ConDep.Dsl.Operations.Application.Deployment.NServiceBus
{
    public class NServiceBusOptions : IOfferNServiceBusOptions
    {
        private readonly NServiceBusOperation _nservicebusProvider;

        public NServiceBusOptions(NServiceBusOperation nservicebusProvider)
        {
            _nservicebusProvider = nservicebusProvider;
        }

        public IOfferNServiceBusOptions ServiceInstaller(string nServiceBusInstallerPath)
    	{
    		_nservicebusProvider.ServiceInstallerName = nServiceBusInstallerPath;
			return this;
    	}

        public IOfferNServiceBusOptions UserName(string username)
    	{
    		_nservicebusProvider.ServiceUserName = username;
    		return this;
    	}

        public IOfferNServiceBusOptions Password(string password)
    	{
    		_nservicebusProvider.ServicePassword = password;
			return this;
    	}


        public IOfferNServiceBusOptions ServiceGroup(string group)
    	{
    		_nservicebusProvider.ServiceGroup = group;
			return this;
    	}

        public IOfferNServiceBusOptions Profile(string profile)
        {
            _nservicebusProvider.Profile = profile;
            return this;
        }

        public IOfferNServiceBusOptions ServiceFailureResetInterval(int interval)
        {
            _nservicebusProvider.ServiceFailureResetInterval = interval;
            return this;
        }

        public IOfferNServiceBusOptions ServiceRestartDelay(int delay)
        {
            _nservicebusProvider.ServiceRestartDelay = delay;
            return this;
        }

        public IOfferNServiceBusOptions IgnoreFailureOnServiceStartStop(bool value)
        {
            _nservicebusProvider.IgnoreFailureOnServiceStartStop = value;
            return this;
        }
    }
}