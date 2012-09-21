using System.IO;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus
{
    public class NServiceBusProvider : WebDeployCompositeProviderBase
    {
		internal const string SERVICE_CONTROLLER_EXE = @"C:\WINDOWS\system32\sc.exe";
        private string _serviceInstallerName = "NServiceBus.Host.exe";

		public NServiceBusProvider(string path, string destDir, string serviceName)
		{
            SourcePath = Path.GetFullPath(path);
            ServiceName = serviceName;
		    DestinationPath = destDir;
		}

    	public string ServiceName { get; set; }
    	public string ServiceGroup { get; set; }
    	public string Password { get; set; }
    	public string UserName { get; set; }
        public string Profile { get; set; }
        public int ServiceFailureResetInterval { get; set; }
        public int ServiceRestartDelay { get; set; }

        public string ServiceInstallerName
        {
            get { return _serviceInstallerName; }
            set { _serviceInstallerName = value; }
        }

        public override void Configure(DeploymentServer server)
        {
            var destinationPath = DestinationPath ?? SourcePath;

            var stop = string.Format("stop-service {0}", ServiceName);
            //Todo: Remove Frende specific things
            var install = string.Format("{0} /install /serviceName:\"{1}\" /displayName:\"{1}\" {2}", Path.Combine(destinationPath, ServiceInstallerName), ServiceName, Profile);
            var failureConfig = string.Format("{0} failure \"{1}\" reset= {2} actions= restart/{3}", SERVICE_CONTROLLER_EXE, ServiceName, ServiceFailureResetInterval, ServiceRestartDelay);
            var userConfig = string.Format("{0} config \"{1}\" obj= \"{2}\" password= \"{3}\" group= \"{4}\"", SERVICE_CONTROLLER_EXE, ServiceName, UserName, Password, ServiceGroup);
            var start = string.Format("start-service {0}", ServiceName);

            Configure<ProvideForInfrastructure>(server, po => po.PowerShell(stop, o => o.ContinueOnError().WaitIntervalInSeconds(10)));
            Configure<ProvideForDeployment>(server, po => po.CopyDir(SourcePath, destinationPath));

            //Allow continue on error??
            Configure<ProvideForInfrastructure>(server, po =>
            {
                po.RunCmd(install);
                po.RunCmd(failureConfig);
                po.RunCmd(userConfig);
                po.PowerShell(start, o => o.WaitIntervalInSeconds(10));
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