using System;

namespace ConDep.Dsl.Model.Config
{
    public class ConDepNoServersFoundException : Exception
    {
        public ConDepNoServersFoundException(string message) : base(message) {}
    }
}