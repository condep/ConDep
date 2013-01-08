using System.Security.Cryptography.X509Certificates;
using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;

namespace IntegrationTests
{
    public class MyWebApplication : ApplicationArtifact, IDependOnInfrastructure<WebServerInfrastructure>//, IDependOnApplication<RaadgivewerbLiv>
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepConfig config)
        {
            //onLocalMachine.ExecuteWebRequest("GET", "http://www.con-dep.net");
            //onLocalMachine
            //    .PreCompile("MyWebApplication", @"C:\MyWebApp", @"C:\_precompiled\MyWebApp")
            //    .TransformConfigFile(@"C:\MyWebApp\", "web.config", "web.prod.config");

            //fromLocalMachine.ToSpecificServer("MyServer", x => x.Deploy.SslCertificate.FromFile());
            onLocalMachine.ToEachServer(x =>
                                            {
                                                x.ExecuteRemote.PowerShell("ipconfig");
                                                //x.Deploy.SslCertificate.FromFile(@"C:\GitHub\ConDep\ConDep.Dsl.Tests\testcert.con-dep.net.pfx", "ConDep");
                                                //x.Deploy.SslCertificate.FromStore(X509FindType.FindBySubjectName, "test");
                                                //x.Deploy
                                                //    .Directory(@"C:\website1", @"C:\Temp\ConDep\MyWebApp");

                                                //x.ExecuteRemote.PowerShell("ipconfig", o => o.WaitIntervalInSeconds(10));
                                                //x.Deploy.NServiceBusEndpoint(@"C:\website1", @"C:\Temp\ConDep\NSB", "MyService");

                                                //.NServiceBusEndpoint("", "", "", opt => opt.Profile(""));

                                                //x.ExecuteRemote
                                                //    .PowerShell();        

                                                //x.Deploy
                                                //    .SslCertificate.FromFile();

                                                //x.FromLocalMachineToServer
                                                //    .ExecuteWebRequest("GET", "http://www.con-dep.net")
                                                //    .ExecuteWebRequest("GET", "http://www.google.com");

                                                //x.Deploy
                                                //    .Directory(@"C:\MyWebApp", @"E:\SomeWebSite\MyWebApp")
                                                //    .Directory(@"C:\temp", @"c:\temp");

                                                //x.FromLocalMachineToServer
                                                //    .ExecuteWebRequest("GET", "http://www.con-dep.net");
                                            }
                );

            //onLocalMachine.ExecuteWebRequest2("GET", "http://www.con-dep.net");
            //fromLocalMachine.ToEachServer(x => x.Deploy.Directory(@"C:\temp", @"e:\temp"));
        }
    }

    public class WebServerInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require)
        {
            //Requirements:
            //.net 4.0
            //dotNetFx40_Full_setup.exe /norestart /q /log %temp%\dotnet40install.log

            require
                .IIS(opt =>
                         {
                             opt.Include.AspNet();
                             opt.RemoveIfPresent.WindowsAuth();
                         })
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

                .IISWebSite("ConDepWebSite55", 5, opt => opt
                                                             .AddHttpBinding(httpOpt => httpOpt.HostName("www.con-dep.net").Port(8080))
                                                             .AddHttpBinding(httpOpt => httpOpt.HostName("www.con-dep.com").Port(8080))
                                                             //.AddHttpsBinding(httpsOpt => httpsOpt.Certificate("test").Port(8081))
                                                             .AddHttpsBinding(X509FindType.FindBySubjectName, "testcert2.con-dep.net", binding => binding.HostName("www.con-dep.net").Port(8081))
                                                             //.AddHttpsBinding("", binding => binding.HostName("www.con-dep.com").Port(8081))
                                                             //.AddHttpsBinding("", "", binding => binding.HostName("www.con-dep.com").Port(8081))
                //.ApplicationPool("")
                //.WebApp("MyWebApp")
                //.WebApp("MyOtherWebApp", @"E:\MyOtherWebApp")
                );

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
