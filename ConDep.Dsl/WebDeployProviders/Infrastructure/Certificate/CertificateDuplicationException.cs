using System;

namespace ConDep.Dsl
{
    public class CertificateDuplicationException : Exception
    {
        public CertificateDuplicationException(string message) : base(message) {}
    }
}