using System;
using System.Diagnostics;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Specs.Executors
{
    public class WebSiteExecutor : ConDepOperation, IExecuteWebDeploy
    {
        public WebDeploymentStatus Execute()
        {
            return Setup(setup => setup.WebDeploy(s => s
                                                           .WithConfiguration(c =>
                                                                                  {
                                                                                      c.DoNotAutoDeployAgent();
                                                                                      c.UseWhatIf();
                                                                                  })
                                                           .From.LocalHost()
                                                           .UsingProvider(p => p
                                                                                   .WebSite("Default Web Site",
                                                                                            "Default Web Site 2")
                                                                                   .Exclude.AppPools()
                                                                                   .Certificates()
                                                                                   .CertificatesOnIisBindings()
                                                                                   .Content()
                                                                                   .FrameworkConfig())
                                                           .To.LocalHost()
                                      ));
        }

        public WebDeploymentStatus ExecuteFromPackage()
        {
            throw new NotImplementedException();
        }

        protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }

        protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
        {
            Trace.TraceInformation(e.Message);
        }
    }
}