using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl
{
    public class ConDepInvalidSetupException : Exception
    {
        public ConDepInvalidSetupException() {}

        public ConDepInvalidSetupException(string message) : base(message) {}

        public ConDepInvalidSetupException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepInvalidSetupException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}