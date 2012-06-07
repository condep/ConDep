using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Providers.IIS.Binding
{
    public class HttpBindingProvider : CompositeProvider
    {
        public HttpBindingProvider(int port)
        {
            throw new NotImplementedException();
        }

        public HttpBindingProvider(int port, Action<BindingOptions> bindingOptions)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Notification notification)
        {
            throw new NotImplementedException();
        }

        public override void Configure()
        {
            throw new NotImplementedException();
        }
    }
}