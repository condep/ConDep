using ConDep.Dsl;
using ConDep.Dsl.Config;

namespace ConDepSamples.DotNetWebAppWithInfrastructure
{
    public class DotNetWebApplication : ApplicationArtifact, IDependOnInfrastructure<WebServerInfrastructure>
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepConfig config)
        {
            //Deploy a Web Application to remote server(s)
            onLocalMachine.ToEachServer
            (
                server => server.Deploy.IisWebApplication
                (
                    sourceDir:      @"..\..\..\SampleApps\AspNetWebFormApp", 
                    webAppName:     "AspNetWebFormApp", 
                    webSiteName:    "ConDepSamples"
                )
            );

            //Test that the Web Application works by executing a HTTP GET (failes if not HTTP Code 200 is returned)
            onLocalMachine.HttpGet
            (
                 url: string.Format("http://{0}:8082/AspNetWebFormApp/", config.Servers[0].Name)
            );
        }
    }
}