using System;

namespace ConDep.Dsl.Remote
{
    public class ConDepNodeValidationException : Exception
    {
        public ConDepNodeValidationException(string message) : base(message) {}
    }
}