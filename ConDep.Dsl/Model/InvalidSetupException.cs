using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl
{
    public class InvalidSetupException : Exception
    {
        public InvalidSetupException() {}

        public InvalidSetupException(string message) : base(message) {}

        public InvalidSetupException(string message, Exception innerException) : base(message, innerException) {}

        public InvalidSetupException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}