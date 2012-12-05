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
                                        x.Deploy.SslCertificate.FromFile(@"C:\GitHub\ConDep\ConDep.Dsl.Tests\testcert.con-dep.net.pfx", "ConDep");
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
        protected override void Configure(IOfferInfrastructure require)
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
