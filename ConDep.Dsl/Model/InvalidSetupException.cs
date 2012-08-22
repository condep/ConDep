using System;

namespace ConDep.Dsl.Core
{
    public class InvalidSetupException : Exception
    {
        public InvalidSetupException(string message, Exception innerException) : base(message, innerException) {}
    }
}