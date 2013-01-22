using System;
using System.Diagnostics;

namespace ConDep.Console
{
    internal sealed class CommandLineParams
    {
        public string AssemblyName { get; set; }

        public string Server { get; set; }

        public string Context { get; set; }

        public TraceLevel TraceLevel { get; set; }

        public bool InfraOnly { get; set; }

        public bool DeployOnly { get; set; }

        public string Environment { get; set; }

        public bool ShowHelp { get; set; }

        public bool BypassLB { get; set; }

        public bool WebDeployExist { get; set; }

        public bool StopAfterMarkedServer { get; set; }

        public bool ContinueAfterMarkedServer { get; set; }
    }
}