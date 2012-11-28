using ConDep.Dsl.Experimental.Application;
using ConDep.Dsl.Experimental.Application.Infrastructure;

namespace IntegrationTests
{
    public class MyWebApplication : ApplicationArtifact, IDependOnInfrastructure<WebServerInfrastructure>//, IDependOnApplication<RaadgivewerbLiv>
    {
        public override void Configure(IOfferApplicationOps onLocalMachine)
        {
            onLocalMachine.ExecuteWebRequest("GET", "http://www.con-dep.net");
            //onLocalMachine
            //    .PreCompile("MyWebApplication", @"C:\MyWebApp", @"C:\_precompiled\MyWebApp")
            //    .TransformConfigFile(@"C:\MyWebApp\", "web.config", "web.prod.config");

            //fromLocalMachine.ToSpecificServer("MyServer", x => x.Deploy.SslCertificate.FromFile());
            onLocalMachine.ToEachServer(x =>
                                    {
                                        x.Deploy
                                            .Directory(@"C:\website1", @"C:\Temp\ConDep\MyWebApp");

                                        x.Deploy
                                            .Directory2(@"C:\website1", @"C:\Temp\ConDep\MyWebApp");

                                            //.NServiceBusEndpoint("", "", "", opt => opt.Profile(""));

                                        //x.ExecuteRemote
                                        //    .Powershell();

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

            onLocalMachine.ExecuteWebRequest2("GET", "http://www.con-dep.net");
            //fromLocalMachine.ToEachServer(x => x.Deploy.Directory(@"C:\temp", @"e:\temp"));
        }
    }

    public class WebServerInfrastructure : InfrastructureArtifact
    {
        protected override void Configure(IConfigureInfrastructure require)
        {
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
