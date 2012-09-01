using System;
using System.Runtime.Serialization;

namespace ConDep.Dsl
{
    public class WebConfigTransformException : Exception
    {
        public WebConfigTransformException() {}

        public WebConfigTransformException(string message) : base(message) {}

        public WebConfigTransformException(string message, Exception innerException) : base(message, innerException) {}

        public WebConfigTransformException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}