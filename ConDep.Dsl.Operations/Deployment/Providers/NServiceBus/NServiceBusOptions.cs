namespace ConDep.Dsl
{
    public class NServiceBusOptions
    {
        private readonly NServiceBusProvider _nservicebusProvider;

        public NServiceBusOptions(NServiceBusProvider nservicebusProvider)
        {
            _nservicebusProvider = nservicebusProvider;
        }
        
        public NServiceBusOptions DestinationDir(string path)
        {
            _nservicebusProvider.DestinationPath = path;
            return this;
        }

    	public NServiceBusOptions ServiceInstaller(string nServiceBusInstallerPath)
    	{
    		_nservicebusProvider.ServiceInstallerName = nServiceBusInstallerPath;
			return this;
    	}

    	public NServiceBusOptions UserName(string username)
    	{
    		_nservicebusProvider.UserName = username;
    		return this;
    	}

    	public NServiceBusOptions Password(string password)
    	{
    		_nservicebusProvider.Password = password;
			return this;
    	}

    	public NServiceBusOptions ServiceGroup(string group)
    	{
    		_nservicebusProvider.ServiceGroup = group;
			return this;
    	}
    }
}