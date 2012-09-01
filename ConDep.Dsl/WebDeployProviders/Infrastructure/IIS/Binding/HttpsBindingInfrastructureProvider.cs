using System;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.Binding
{
    public class HttpsBindingInfrastructureProvider : WebDeployCompositeProviderBase
    {
        public HttpsBindingInfrastructureProvider(int port, string certificateCommonName)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Notification notification)
        {
            throw new NotImplementedException();
        }

        public override void Configure(DeploymentServer server)
        {
            throw new NotImplementedException();
        }
    }
}