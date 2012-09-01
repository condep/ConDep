using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.Certificate
{
    public class CertificateDuplicationException : Exception
    {
        public CertificateDuplicationException() {}

        public CertificateDuplicationException(string message) : base(message) {}

        public CertificateDuplicationException(string message, Exception innerException) : base(message, innerException) {}

        public CertificateDuplicationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}