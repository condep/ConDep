using System;

namespace ConDep.Dsl.SemanticModel
{
    public class ConDepClientValidationException : Exception
    {
        public ConDepClientValidationException(string message) : base(message) {}
    }
}