using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeploy
{
    public class UnsupportedOutcomeException : Exception
    {
        public UnsupportedOutcomeException() {}

        public UnsupportedOutcomeException(string message) : base(message) {}

        public UnsupportedOutcomeException(string message, Exception innerException) : base(message, innerException) {}

        public UnsupportedOutcomeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}