using System;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class DotNetFrameworkVersion
    {
        public bool Installed { get; set; }

        public string Version { get; set; }

        public int? ServicePack { get; set; }

        public int? Release { get; set; }

        public string TargetVersion { get; set; }

        public bool Client { get; set; }

        public bool Full { get; set; }
    }
}