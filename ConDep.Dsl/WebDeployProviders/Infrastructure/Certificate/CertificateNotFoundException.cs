using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.Certificate
{
    public class CertificateNotFoundException : Exception
    {
        public CertificateNotFoundException() {}

        public CertificateNotFoundException(string message) : base(message) {}

        public CertificateNotFoundException(string message, Exception innerException) : base(message, innerException) {}

        public CertificateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}