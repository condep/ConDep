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
                    .RecycleTimeInMinutes(0))
                //.IISWebApp("MyWebApp", "ConDepWebSite55")
                .IISWebSite("ConDepWebSite55", 5, opt => opt
                                                             .AddHttpBinding(httpOpt => httpOpt.HostName("www.con-dep.net").Port(8080))
                                                             .AddHttpBinding(httpOpt => httpOpt.HostName("www.con-dep.com").Port(8080))
                                                             .AddHttpsBinding(X509FindType.FindBySubjectName, "testcert2.con-dep.net", binding => binding.HostName("www.con-dep.net").Port(8081))
                                                             .ApplicationPool("Bogus")
                                                             .WebApp("MyWebApp", webAppOpt => webAppOpt.PhysicalPath(@"C:\temp\MyWebApp"))
                    )
                //.IISWebApp("MyWebApp", "ConDepWebSite55")
                ;





















                //.IIS(opt =>
                //         {
                //             opt.Include.AspNet();
                //         })
                //.IISAppPool("MyAppPool", opt =>
                //                             {
                //                                 opt.LoadUserProfile = true;
                //                                 opt.NetFrameworkVersion = NetFrameworkVersion.Net4_0;
                //                                 opt.RecycleTimeInMinutes = 0;
                //                             }
                //)
                                                             
                                                             //.AddHttpsBinding("", binding => binding.HostName("www.con-dep.com").Port(8081))
                //.AddHttpsBinding("", "", binding => binding.HostName("www.con-dep.com").Port(8081))
                //.ApplicationPool("")
                //.WebApp("MyWebApp")
                //.WebApp("MyOtherWebApp", @"E:\MyOtherWebApp")
                //);

            //require.IISAppPool("MyAppPool", opt =>
            //                                    {
            //                                        opt.LoadUserProfile = true;
            //                                    });
            //require
            //    .IIS(opt =>
            //             {
            //                 opt.Include
            //                     .HttpRedirect()
            //                     .AspNet()
            //                     .CustomLogging()
            //                     .BasicAuth()
            //                     .WindowsAuth();
            //                 opt.RemoveIfPresent
            //                     .CertAuth()
            //                     .UrlAuth();
            //             });
            //.IISWebSite("MyWebSite", 1)
            //.IISAppPool("MyAppPool");
            //.Include
            //    .HttpRedirect()
            //    .AspNet()
            //    .CustomLogging()
            //    .BasicAuth()
            //    .WindowsAuth();
            //.RemoveIfExist
            //    .HttpRedirect();




            //require.Iis();
            //require.IisWebSite()
            //        .ApplicationPool()
            //        .WebApplication()
            //        .WebApplication();

            //            .ApplicationPool()
            //        .WebApplication()
            //    .WebSite()
            //        .WebApplication()
            //        .WebApplication();

            //require.Msmq();            
        }
    }
}