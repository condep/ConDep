using System;

namespace ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus
{
    public class ConDepResourceNotFoundException : Exception
    {
        public ConDepResourceNotFoundException(string message, Exception innerException) : base(message, innerException) {}
    }
}