using System;

namespace ConDep.Dsl.SemanticModel
{
    public class ConDepValidationException : Exception
    {
        public ConDepValidationException(string message) : base(message) {}
    }
}