using System;

namespace ConDep.Dsl
{
    internal class UnknowNetFrameworkException : Exception
    {
        public UnknowNetFrameworkException(string message) : base(message) {}
    }
}