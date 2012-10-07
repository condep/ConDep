using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeployProviders.RunCmd
{
    internal class ConDepUntrappedExitCodeException : Exception
    {
        public ConDepUntrappedExitCodeException() {}

        public ConDepUntrappedExitCodeException(string message) : base(message) {}

        public ConDepUntrappedExitCodeException(string message, Exception innerException) : base(message, innerException) {}

        public ConDepUntrappedExitCodeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}