using System;

namespace ConDep.Dsl
{
    public class InvalidSetupException : Exception
    {
        public InvalidSetupException(string message, Exception innerException) : base(message, innerException) {}
    }
}