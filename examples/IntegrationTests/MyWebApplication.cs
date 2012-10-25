using ConDep.Dsl.Application;
using ConDep.Dsl.Application.Infrastructure;

namespace IntegrationTests
{
    public class MyWebApplication : ApplicationArtifact<RequireWebServer>
    {
        protected override void Configure(IDeploy deploy, IExecute execute, IExecuteLocally locally)
        {
            locally.TransformConfigFile(@"C:\MyWebApp\", "web.config", "web.prod.config");
            locally.PreCompile("MyWebApplication", @"C:\MyWebApp", @"C:\_precompiled\MyWebApp");
            locally.ExecuteWebRequest("GET", "http://www.con-dep.net");

            deploy.Directory(@"C:\MyWebApp", @"E:\SomeWebSite\MyWebApp");
            deploy.File(@"C:\SomeDir\SomeFile.txt", @"E:\SomeWebSite\MyWebApp\SomeFile.txt");
            deploy.IisWebApplication();
            deploy.WindowsService();
            deploy.NServiceBusEndpoint("", "", "");
            deploy.NServiceBusEndpoint("", "", "", opt => opt.Profile(""));
            deploy.SslCertificate.FromStore();
            deploy.SslCertificate.FromFile();

            execute.DosCommand();
            execute.Powershell();
        }
    }

    public class RequireWebServer : InfrastructureArtifact
    {
        protected override void Configure(IRequireInfrastructure require)
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
