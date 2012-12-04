using System;

namespace ConDep.Dsl.Config
{
    public class ConDepNoServersFoundException : Exception
    {
        public ConDepNoServersFoundException(string message) : base(message) {}
    }
}