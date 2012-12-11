using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;

namespace IntegrationTests
{
    public class SomeWebApplication : 
        ApplicationArtifact, 
        IDependOnInfrastructure<MyServerInfrastructure>
    {
        public override void Configure(IOfferLocalOperations local, ConDepConfig config)
        {
            local.ToEachServer(x => x.Deploy.Directory(@"C:\SomeWebApp", @"E:\WebApps\SomeWebApp"));
        }
    }

    public class MyServerInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require)
        {
            //require.IIS(TODO)
            //    .Include
            //        .HttpRedirect()
            //        .DAVPublishing()
            //        .AspNet()
            //        .ASP()
            //        .CGI()
            //        .ServerSideIncludes()
            //        .LogLibraries()
            //        .HttpTracing()
            //        .CustomLogging()
            //        .ODBCLogging()
            //        .BasicAuth()
            //        .WindowsAuth()
            //        .DigestAuth()
            //        .ClientAuth()
            //        .CertAuth()
            //        .UrlAuth()
            //        .IPSecurity()
            //        .DynamicCompression()
            //        .ScriptingTools()
            //        .MgmtService()
            //    .RemoveIfExist
            //        .HttpRedirect();

            //require.IISWebSite("WebSiteName", 5)
            //    .WebApp("MyWebApp");

            //require.IISAppPool("MyAppPool");

            //require.MSMQ();
        }
    }
}