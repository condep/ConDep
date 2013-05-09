using System;

namespace ConDep.Node
{
    public class InvalidAttributesException : Exception
    {
        public InvalidAttributesException(string message) : base(message) {}
    }
}