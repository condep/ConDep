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
        public int? ServiceFailureResetInterval { get; set; }
        public int? ServiceRestartDelay { get; set; }

        public string ServiceInstallerName
        {
            get { return _serviceInstallerName; }
            set { _serviceInstallerName = value; }
        }

        public override void Configure(DeploymentServer server)
        {
            //var stop = string.Format("stop-service {0}", ServiceName);
            var stop = string.Format("\"Stopping {0}\"; try {{ Get-Service {0} -ErrorAction Stop | ForEach {{ if ($_.Status -eq [System.ServiceProcess.ServiceControllerStatus]::Running) {{ \"Stopping: \" + $_.DisplayName; $_.Stop(); $_.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped); \"Stopped: \" + $_.DisplayName; }} else {{ $_.DisplayName + \" is already stopped\" }}	C:\\WINDOWS\\system32\\sc.exe delete $_.DisplayName }} }} catch {{ \"Service not found {0}\" }}", ServiceName);
            var install = string.Format("{0} /install /serviceName:\"{1}\" /displayName:\"{1}\" {2}", Path.Combine(DestinationPath, ServiceInstallerName), ServiceName, Profile);

            var serviceFailureCommand = "";
            var serviceConfigCommand = "";

            if(HasServiceFailureOptions)
            {
                var serviceResetOption = ServiceFailureResetInterval.HasValue ? "reset= " + ServiceFailureResetInterval.Value : "";
                var serviceRestartDelayOption = ServiceRestartDelay.HasValue ? "actions= restart/" + ServiceRestartDelay.Value : "";
                
                serviceFailureCommand = string.Format("{0} failure \"{1}\" {2} {3}", SERVICE_CONTROLLER_EXE, ServiceName, serviceResetOption, serviceRestartDelayOption);
            }

            if(HasServiceConfigOptions)
            {
                var userNameOption = !string.IsNullOrWhiteSpace(UserName) ? "obj= \"" + UserName + "\"" : "";
                var passwordOption = !string.IsNullOrWhiteSpace(Password) ? "password= \"" + Password + "\"" : "";
                var groupOption = !string.IsNullOrWhiteSpace(ServiceGroup) ? "group= \"" + ServiceGroup + "\"" : "";

                serviceConfigCommand = string.Format("{0} config \"{1}\" {2} {3} {4}", SERVICE_CONTROLLER_EXE, ServiceName, userNameOption, passwordOption, groupOption);
            }

            var start = string.Format("start-service {0}", ServiceName);

            Configure<ProvideForInfrastructure>(server, po => po.PowerShell(stop, o => o.ContinueOnError().WaitIntervalInSeconds(10)));
            Configure<ProvideForDeployment>(server, po => po.CopyDir(SourcePath, DestinationPath));

            //Allow continue on error??
            Configure<ProvideForInfrastructure>(server, po =>
            {
                po.RunCmd(install);
                if(!string.IsNullOrWhiteSpace(serviceFailureCommand)) po.RunCmd(serviceFailureCommand);
                if(!string.IsNullOrWhiteSpace(serviceConfigCommand)) po.RunCmd(serviceConfigCommand);
                po.PowerShell(start, o => o.WaitIntervalInSeconds(10));
            });
        }

        private bool HasServiceConfigOptions
        {
            get { return !string.IsNullOrWhiteSpace(UserName) || !string.IsNullOrWhiteSpace(Password) || !string.IsNullOrWhiteSpace(ServiceGroup); }
        }

        private bool HasServiceFailureOptions
        {
            get { return ServiceFailureResetInterval.HasValue || ServiceRestartDelay.HasValue; }
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