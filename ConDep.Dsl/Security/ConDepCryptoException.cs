using System;

namespace ConDep.Dsl.Security
{
    public class ConDepCryptoException : Exception
    {
        public ConDepCryptoException(string message)
            : base(message)
        {

        }
    }
}