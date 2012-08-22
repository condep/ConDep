using System;

namespace ConDep.Dsl
{
    public class CertificateNotFoundException : Exception
    {
        public CertificateNotFoundException(string message) : base(message) {}
    }
}