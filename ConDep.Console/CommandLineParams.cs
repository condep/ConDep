using System;
using System.Diagnostics;

namespace ConDep.Console
{
    internal class CommandLineParams
    {
        public string AssemblyName { get; set; }

        public string Server { get; set; }

        public string Application { get; set; }

        public TraceLevel TraceLevel { get; set; }

        public bool InfraOnly { get; set; }

        public bool DeployOnly { get; set; }

        public string Environment { get; set; }

        public bool ShowHelp { get; set; }
    }
}