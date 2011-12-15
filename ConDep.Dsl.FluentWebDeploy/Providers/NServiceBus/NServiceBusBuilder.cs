namespace ConDep.Dsl.FluentWebDeploy
{
    public class NServiceBusBuilder
    {
        private readonly NServiceBusProvider _nservicebusProvider;

        public NServiceBusBuilder(NServiceBusProvider nservicebusProvider)
        {
            _nservicebusProvider = nservicebusProvider;
        }
        
        public NServiceBusBuilder ToDirectory(string path)
        {
            _nservicebusProvider.DestinationPath = path;
            return this;
        }

    	public NServiceBusBuilder ServiceName(string serviceName)
    	{
    		_nservicebusProvider.ServiceName = serviceName;
    		return this;
    	}

    	public NServiceBusBuilder ServiceInstaller(string nServiceBusInstallerPath)
    	{
    		_nservicebusProvider.ServiceInstallerName = nServiceBusInstallerPath;
			return this;
    	}

    	public NServiceBusBuilder UserName(string username)
    	{
    		_nservicebusProvider.UserName = username;
    		return this;
    	}

    	public NServiceBusBuilder Password(string password)
    	{
    		_nservicebusProvider.Password = password;
			return this;
    	}

    	public NServiceBusBuilder ServiceGroup(string group)
    	{
    		_nservicebusProvider.ServiceGroup = group;
			return this;
    	}
    }
}