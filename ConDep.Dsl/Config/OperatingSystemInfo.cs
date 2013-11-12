using System;
using System.Globalization;

namespace ConDep.Dsl.Config
{
    public class OperatingSystemInfo
    {
        public TimeSpan SystemUpTime { get; set; }
        public string HostName { get; set; }
        public string SystemType { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int BuildNumber { get; set; }
    }
}