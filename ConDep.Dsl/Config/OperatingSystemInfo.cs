using System;

namespace ConDep.Dsl.Config
{
    public class OperatingSystemInfo
    {
        public TimeSpan SystemUpTime { get; set; }
        public string HostName { get; set; }
        public string SystemType { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
    }
}