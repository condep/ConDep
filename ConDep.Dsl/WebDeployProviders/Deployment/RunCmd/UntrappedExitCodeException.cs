using System;

namespace ConDep.Dsl
{
    internal class UntrappedExitCodeException : Exception
    {
        public UntrappedExitCodeException(string message, Exception innerException) : base(message, innerException) {}
    }
}