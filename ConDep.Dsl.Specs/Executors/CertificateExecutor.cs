using System.Diagnostics;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Specs.Executors
{
    public class CertificateExecutor : ConDepOperation, IExecuteWebDeploy
    {
        private readonly string _certificateThumbprint;

        public CertificateExecutor(string certificateThumbprint)
        {
            _certificateThumbprint = certificateThumbprint;
        }

        public WebDeploymentStatus ExecuteFromPackage()
        {
            return Setup(setup => setup.Sync(s =>
                                                 {
                                                     s.WithConfiguration(c => c.DoNotAutoDeployAgent());
                                                     s.From.Package(@"C:\package.zip", "test123");
                                                     s.Using.Certificate(_certificateThumbprint);
                                                     s.To.LocalHost();
                                                 }
                                      ));

        }

        public WebDeploymentStatus Execute()
        {
            return Setup(setup => setup.Sync(s =>
                                                 {
                                                     s.WithConfiguration(c => c.DoNotAutoDeployAgent());
                                                     s.From.LocalHost();
                                                     s.Using.Certificate(_certificateThumbprint);
                                                     s.To.LocalHost();
                                                 }
                                      ));
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