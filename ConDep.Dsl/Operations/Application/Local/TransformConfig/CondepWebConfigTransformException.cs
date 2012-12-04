using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl.Operations.Application.Local.TransformConfig
{
    public class CondepWebConfigTransformException : Exception
    {
        public CondepWebConfigTransformException() {}

        public CondepWebConfigTransformException(string message) : base(message) {}

        public CondepWebConfigTransformException(string message, Exception innerException) : base(message, innerException) {}

        public CondepWebConfigTransformException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}