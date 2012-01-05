using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
    public class NServiceBusProvider : CompositeProvider
    {
		internal const string SERVICE_CONTROLLER_EXE = @"C:\WINDOWS\system32\sc.exe";
        private string _serviceInstallerName = "NServiceBus.Host.exe";

		public NServiceBusProvider(string path, string serviceName)
        {
            SourcePath = path;
		    ServiceName = serviceName;
        }

    	public string ServiceName { get; set; }
    	public string ServiceGroup { get; set; }
    	public string Password { get; set; }
    	public string UserName { get; set; }

        public string ServiceInstallerName
        {
            get { return _serviceInstallerName; }
            set { _serviceInstallerName = value; }
        }

        public override void Configure()
    	{
    	    var destinationPath = DestinationPath ?? SourcePath;

			var stop = string.Format("stop-service {0}", ServiceName);
			var install = string.Format("{0} /install /serviceName:\"{1}\" /displayName:\"{1}\" NServiceBus.Frende", System.IO.Path.Combine(destinationPath, ServiceInstallerName), ServiceName);
			var failureConfig = string.Format("{0} failure \"{1}\" reset= 300 actions= restart/5000", SERVICE_CONTROLLER_EXE, ServiceName);
			var userConfig = string.Format("{0} config \"{1}\" obj= \"{2}\" password= \"{3}\" group= {4}", SERVICE_CONTROLLER_EXE, ServiceName, UserName, Password, ServiceGroup);
			var start = string.Format("start-service {0}", ServiceName);

			Configure(p =>
			            {
			            	p.PowerShell(stop);
                            p.CopyDir(SourcePath, c => c.DestinationDir(destinationPath));
			            	p.RunCmd(install);
			            	p.RunCmd(failureConfig);
			            	p.RunCmd(userConfig);
			            	p.PowerShell(start);
			            });
		}

    	public override bool IsValid(Notification notification)
        {
            var valid = true;
            foreach (var childProvider in ChildProviders)
            {
                if(!childProvider.IsValid(notification))
                {
                    valid = false;
                }
            }


            return valid;
        }
    }
}