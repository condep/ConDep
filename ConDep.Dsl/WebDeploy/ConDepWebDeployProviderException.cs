using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeploy
{
    public class ConDepWebDeployProviderException : Exception
    {
        public ConDepWebDeployProviderException() {}

        public ConDepWebDeployProviderException(string message) : base(message) {}

        public ConDepWebDeployProviderException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepWebDeployProviderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}