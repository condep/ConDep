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

        public WindowsOperatingSystem OsAsEnum()
        {
            switch (BuildNumber)
            {
                case 6001: return WindowsOperatingSystem.WindowsServer2008;
                case 7600: return WindowsOperatingSystem.WindowsServer2008R2;
                case 7601: return WindowsOperatingSystem.WindowsServer2008R2_SP1;
                case 9200: return WindowsOperatingSystem.WindowsServer2012;
                default: return WindowsOperatingSystem.Unknown;
            }
        }
    }
}