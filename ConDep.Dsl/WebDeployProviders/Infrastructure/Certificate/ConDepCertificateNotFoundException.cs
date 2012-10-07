using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeployProviders.Infrastructure.Certificate
{
    public class ConDepCertificateNotFoundException : Exception
    {
        public ConDepCertificateNotFoundException() {}

        public ConDepCertificateNotFoundException(string message) : base(message) {}

        public ConDepCertificateNotFoundException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepCertificateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}