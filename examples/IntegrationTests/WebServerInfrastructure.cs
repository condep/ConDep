using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Infrastructure.IIS;

//Requirements:
//.net 4.0
//dotNetFx40_Full_setup.exe /norestart /q /log %temp%\dotnet40install.log

namespace IntegrationTests
{
    public class WebServerInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require, ConDepConfig config)
        {
            require
                .IIS()
                .IISAppPool("Bogus", options => options
                    .Enable32Bit(false)
                    .IdleTimeoutInMinutes(0)
                    .LoadUserProfile(true)
                    .ManagedPipeline(ManagedPipeline.Integrated)
                    .NetFrameworkVersion(NetFrameworkVersion.Net4_0)
                    .RecycleTimeInMinutes(0)
                )
                .SslCertificate.FromFile(@"C:\somecert.pfx", "12345", certOpt => certOpt.AddPrivateKeyPermission("torresdal\\condeptest"))
                //.SslCertificate.FromStore(X509FindType.FindByThumbprint, "cb 5f 27 74 dc 0a 00 65 ba 5a ab 23 b4 63 ab 3f 9c 48 8e 5c", certOpt => certOpt.AddPrivateKeyPermission("z63\\__104171dep"))
                //.IISWebSite("con.dep.site", 5, opt => opt
                //                                             .AddHttpBinding(httpOpt => httpOpt
                //                                                 .HostName("www.con-dep.net")
                //                                                 .Port(80))
                //                                             .AddHttpsBinding(
                //                                                X509FindType.FindByThumbprint,
                //                                                "cb 5f 27 74 dc 0a 00 65 ba 5a ab 23 b4 63 ab 3f 9c 48 8e 5c",
                //                                                binding => binding.HostName("www.con-dep.net").Port(443)
                //                                             )
                //                                             .ApplicationPool("Bogus")
                //                                             .WebApp("MyWebApp", webAppOpt => webAppOpt
                //                                                 .PhysicalPath(@"C:\temp\MyWebApp"))
                //    );
                .IISWebSite("condep.site", 6, opt => opt.AddHttpBinding(binding => binding.Port(8080)))
                .IISWebApp("TestApp", "condep.site");
        }
    }
}