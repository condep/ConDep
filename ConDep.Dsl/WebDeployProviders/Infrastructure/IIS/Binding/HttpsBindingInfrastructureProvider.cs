using System;
using ConDep.Dsl;

namespace ConDep.Dsl
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