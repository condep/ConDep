using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Infrastructure.IIS;

//Requirements:
//.net 4.0
//dotNetFx40_Full_setup.exe /norestart /q /log %temp%\dotnet40install.log

namespace IntegrationTests
{
    public class WebServerInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require)
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
                .IISWebSite("ConDepWebSite55", 5, opt => opt
                                                             .AddHttpBinding(httpOpt => httpOpt
                                                                 .HostName("www.con-dep.net")
                                                                 .Port(80))
                                                             .AddHttpsBinding(X509FindType.FindBySubjectName, "testcert2.con-dep.net", binding => binding
                                                                    .HostName("www.con-dep.net")
                                                                    .Port(443))
                                                             .ApplicationPool("Bogus")
                                                             .WebApp("MyWebApp", webAppOpt => webAppOpt
                                                                 .PhysicalPath(@"C:\temp\MyWebApp"))
                    );
        }
    }
}