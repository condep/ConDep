using System;

namespace ConDep.Dsl.Resources
{
    public class ConDepResourceNotFoundException : Exception
    {
        public ConDepResourceNotFoundException(string message, Exception innerException) : base(message, innerException) {}
    }
}