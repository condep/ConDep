using System.Diagnostics;
using System.Reflection;
using ConDep.Dsl.Operations.LoadBalancer;

namespace ConDep.Dsl.Config
{
    public class ConDepOptions
    {
        public string Application { get; set; }
        public bool DeployOnly { get; set; }
        public bool StopAfterMarkedServer { get; set; }
        public bool ContinueAfterMarkedServer { get; set; }
        public bool DeployAllApps { get; set; }
        public Assembly Assembly { get; set; }
        public LoadBalancerSuspendMethod SuspendMode { get; set; }
        public TraceLevel TraceLevel { get; set; }
        public string WebQAddress { get; set; }
        public bool BypassLB { get; set; }
        public bool InstallWebQ { get; set; }
        public string InstallWebQOnServer { get; set; }
        public string Environment { get; set; }
        public string AssemblyName { get; set; }
        public string CryptoKey { get; set; }

        public bool HasApplicationDefined()
        {
            return !string.IsNullOrWhiteSpace(Application);
        }
    }
}