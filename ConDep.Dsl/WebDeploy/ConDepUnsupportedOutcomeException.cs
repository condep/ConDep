using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeploy
{
    public class ConDepUnsupportedOutcomeException : Exception
    {
        public ConDepUnsupportedOutcomeException() {}

        public ConDepUnsupportedOutcomeException(string message) : base(message) {}

        public ConDepUnsupportedOutcomeException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepUnsupportedOutcomeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}