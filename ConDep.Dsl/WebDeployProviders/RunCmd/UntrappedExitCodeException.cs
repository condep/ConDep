using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.WebDeployProviders.RunCmd
{
    internal class UntrappedExitCodeException : Exception
    {
        public UntrappedExitCodeException() {}

        public UntrappedExitCodeException(string message) : base(message) {}

        public UntrappedExitCodeException(string message, Exception innerException) : base(message, innerException) {}

        public UntrappedExitCodeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}