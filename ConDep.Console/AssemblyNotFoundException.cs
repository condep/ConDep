using System;
using System.Runtime.Serialization;

namespace ConDep.Console
{
    public class AssemblyNotFoundException : Exception
    {
        public AssemblyNotFoundException() {}

        public AssemblyNotFoundException(string message) : base(message) {}

        public AssemblyNotFoundException(string message, Exception innerException) : base(message, innerException) {}

        public AssemblyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}