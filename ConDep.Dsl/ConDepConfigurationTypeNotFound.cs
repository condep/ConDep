using System;

namespace ConDep.Dsl
{
    internal class ConDepConfigurationTypeNotFound : Exception
    {
        public ConDepConfigurationTypeNotFound(string message) : base(message) {}
    }
}