using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;

namespace IntegrationTests
{
    public class MyWebApplication : ApplicationArtifact//, IDependOnInfrastructure<WebServerInfrastructure>//, IDependOnApplication<RaadgivewerbLiv>
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepConfig config)
        {
            onLocalMachine.ToEachServer(x =>
                                            {
                                                x.ExecuteRemote.DosCommand("echo 'test'");
                                            }
                );












            //x.Deploy
            //    .SslCertificate.FromFile(@"C:\GitHub\ConDep\ConDep.Dsl.Tests\testcert.con-dep.net.pfx", "ConDep")
            //    .SslCertificate.FromStore(X509FindType.FindBySubjectName, "testcert2");
            //.SslCertificate.Credentials(X509FindType.FindByThumbprint, "a848738979475", @"z63\__104171test", UR.Read);

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



            //onLocalMachine.ExecuteWebRequest("GET", "http://www.con-dep.net");
            //onLocalMachine
            //    .PreCompile("MyWebApplication", @"C:\MyWebApp", @"C:\_precompiled\MyWebApp")
            //    .TransformConfigFile(@"C:\MyWebApp\", "web.config", "web.prod.config");

            //fromLocalMachine.ToSpecificServer("MyServer", x => x.Deploy.SslCertificate.FromFile());

            //onLocalMachine.ExecuteWebRequest2("GET", "http://www.con-dep.net");
            //fromLocalMachine.ToEachServer(x => x.Deploy.Directory(@"C:\temp", @"e:\temp"));
        }
    }
}
