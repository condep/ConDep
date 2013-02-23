using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl;
using ConDep.Dsl.Config;

namespace ConDepSamples.DotNetWebAppWithSSL
{
    public class WebServerSslInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require, ConDepConfig config)
        {
            const string appPoolName = "ConDepSamplesAppPool";

            require
                //Install IIS with Asp.net if not present
                .IIS(iis => iis.Include.AspNet())

                //Add an Application Pool running .NET Framework 4.0 (.NET 4.0 must be installed and registered with IIS)
                .IISAppPool(appPoolName, appPool => appPool.NetFrameworkVersion(NetFrameworkVersion.Net4_0))
                
                //Copies and installs a SSL certificate
                .SslCertificate.FromFile
                (
                    path:       @"DotNetWebAppWithSSL\bogus.con-dep.net.pfx", 
                    password:   "ReallySecureP@ssw0rd :)"
                )
                
                //Create a Web Site (id 34) with an HTTPS binding on port 8083 using the previously installed certificate +
                //a standard HTTP binding on port 8082, and asociate Web Site with application pool.
                .IISWebSite
                (
                    "ConDepSamples", 34, opt => opt
                        .AddHttpBinding(binding => binding.Port(8082))
                        .AddHttpsBinding
                        (
                            X509FindType.FindByThumbprint, 
                            "d8 2a 04 fe 72 20 34 eb 90 45 03 d7 87 16 ce f5 30 c4 b3 ed", 
                            binding => binding.Port(8083)
                        )
                        .ApplicationPool(appPoolName)
                );
        }
    }
}