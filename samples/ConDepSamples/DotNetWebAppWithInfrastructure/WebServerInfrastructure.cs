using ConDep.Dsl;
using ConDep.Dsl.Config;

namespace ConDepSamples.DotNetWebAppWithInfrastructure
{
    public class WebServerInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require, ConDepConfig config)
        {
            require
                //Install IIS with Asp.net if not present
                .IIS(iis => iis.Include.AspNet())

                //Add an Application Pool running .NET Framework 4.0 (.NET 4.0 must be installed and registered with IIS)
                .IISAppPool("ConDepSamplesAppPool", appPool => appPool.NetFrameworkVersion(NetFrameworkVersion.Net4_0))

                //Create a Web Site (id 34) on port 8082 and asociate with application pool.
                .IISWebSite("ConDepSamples", 34, opt => opt
                    .AddHttpBinding(binding => binding.Port(8082))
                    .ApplicationPool("ConDepSamplesAppPool"));
        }
    }
}