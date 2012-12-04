using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.Operations.Infrastructure.Certificate
{
    public class ConDepCertificateDuplicationException : Exception
    {
        public ConDepCertificateDuplicationException() {}

        public ConDepCertificateDuplicationException(string message) : base(message) {}

        public ConDepCertificateDuplicationException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepCertificateDuplicationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}