using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy
{
    public class NServiceBusProvider : CompositeProvider
    {
		internal const string WIN_SERVICE_EXE = @"C:\WINDOWS\system32\sc.exe";
		
		public NServiceBusProvider(string path)
        {
            SourcePath = path;
        }

    	public string ServiceName { get; set; }
    	public string ServiceGroup { get; set; }
    	public string Password { get; set; }
    	public string UserName { get; set; }
    	public string ServiceInstallerName { get; set; }

    	public override void Configure()
		{
			var stop = string.Format("{0} stop {1}", WIN_SERVICE_EXE, ServiceName);
			var install = string.Format("{0} /install /serviceName:\"{1}\" /displayName:\"{1}\" NServiceBus.Frende", System.IO.Path.Combine(DestinationPath, ServiceInstallerName), ServiceName);
			var failureConfig = string.Format("{0} failure \"{1}\" reset= 300 actions= restart/5000", WIN_SERVICE_EXE, ServiceName);
			var userConfig = string.Format("{0} config \"{1}\" obj= \"{2}\" password= \"{3}\" group= {4}", WIN_SERVICE_EXE, ServiceName, UserName, Password, ServiceGroup);
			var start = string.Format("{0} start {1} /wait", WIN_SERVICE_EXE, ServiceName);

			Configure(p =>
			            {
			            	p.RunCmd(stop, c => c.WaitIntervalInSeconds(15));
			            	p.CopyDir(SourcePath, c => c.SetRemotePathTo(DestinationPath));
			            	p.RunCmd(install);
			            	p.RunCmd(failureConfig);
			            	p.RunCmd(userConfig);
			            	p.RunCmd(start);
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