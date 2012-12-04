using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.AppPool
{
    internal class ConDepUnknowNetFrameworkException : Exception
    {
        public ConDepUnknowNetFrameworkException() {}

        public ConDepUnknowNetFrameworkException(string message) : base(message) {}

        public ConDepUnknowNetFrameworkException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepUnknowNetFrameworkException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}