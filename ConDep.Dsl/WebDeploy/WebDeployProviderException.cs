using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeploy
{
    public class WebDeployProviderException : Exception
    {
        public WebDeployProviderException() {}

        public WebDeployProviderException(string message) : base(message) {}

        public WebDeployProviderException(string message, Exception innerException) : base(message, innerException) {}

        public WebDeployProviderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}