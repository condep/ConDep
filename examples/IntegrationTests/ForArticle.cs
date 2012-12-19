using ConDep.Dsl;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;

namespace IntegrationTests
{
    public class ConDepWebApp : ApplicationArtifact, IDependOnInfrastructure<WebServer>
    {
        public override void Configure(IOfferLocalOperations local, ConDepConfig config)
        {
            local.ToEachServer(x => x.Deploy.Directory(@"C:\SomeWebApp", @"E:\WebApps\SomeWebApp"));
        }
    }

    public class WebServer : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require)
        {
            require
                .IIS()
                .IISWebSite("ConDepWebSite", 1)
                .IISAppPool("ConDepAppPool");
            //.IISWebApp("ConDep");
        }
    }
}