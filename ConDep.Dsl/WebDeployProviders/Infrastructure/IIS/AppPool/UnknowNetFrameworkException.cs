using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.IIS.AppPool
{
    internal class UnknowNetFrameworkException : Exception
    {
        public UnknowNetFrameworkException() {}

        public UnknowNetFrameworkException(string message) : base(message) {}

        public UnknowNetFrameworkException(string message, Exception innerException) : base(message, innerException) {}

        public UnknowNetFrameworkException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}