using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.Binding
{
    public class HttpsBindingInfrastructureProvider : RemoteCompositeOperation
    {
        public HttpsBindingInfrastructureProvider(int port, string certificateCommonName)
        {
            throw new NotImplementedException();
        }

        public override void Configure(IOfferRemoteOperations server)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}