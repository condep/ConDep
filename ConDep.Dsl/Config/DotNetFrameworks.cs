using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl.Config
{
    public class DotNetFrameworks : IEnumerable<DotNetFrameworkVersion>
    {
        private List<DotNetFrameworkVersion> _versions = new List<DotNetFrameworkVersion>(); 
        public void Add(dynamic dotNetVersion)
        {
            _versions.Add(new DotNetFrameworkVersion
                {
                    Installed = Convert.ToBoolean(dotNetVersion.Installed),
                    Version = dotNetVersion.Version,
                    ServicePack = dotNetVersion.ServicePack,
                    Release = dotNetVersion.Release,
                    TargetVersion = dotNetVersion.TargetVersion,
                    Client = dotNetVersion.Client,
                    Full = dotNetVersion.Full
                });
            var version = dotNetVersion.Version;

        }

        public DotNetFrameworkVersion this[DotNetVersion version]
        {
            get
            {
                switch (version)
                {
                    case DotNetVersion.v1_1:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("1.1"));
                    case DotNetVersion.v2_0:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("2.0"));
                    case DotNetVersion.v3_0:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("3.0"));
                    case DotNetVersion.v3_5:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("3.5"));
                    case DotNetVersion.v4_0:
                        return _versions.SingleOrDefault(x => x.TargetVersion == "4.0.0");
                    case DotNetVersion.v4_0_client:
                        return _versions.SingleOrDefault(x => x.TargetVersion == "4.0.0" && x.Client);
                    case DotNetVersion.v4_0_full:
                        return _versions.SingleOrDefault(x => x.TargetVersion == "4.0.0" && x.Full);
                    case DotNetVersion.v4_5:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("4.5"));
                    case DotNetVersion.v4_5_client:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("4.5") && x.Client);
                    case DotNetVersion.v4_5_full:
                        return _versions.SingleOrDefault(x => x.Version.StartsWith("4.5") && x.Full);
                    default:
                        return null;
                }
            }
        } 

        public bool HasVersion(DotNetVersion version)
        {
            return this[version] != null;
        }

        public IEnumerator<DotNetFrameworkVersion> GetEnumerator()
        {
            return _versions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}