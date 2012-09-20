using System;

namespace ConDep.Console
{
    internal class NoServersFoundException : Exception
    {
        public NoServersFoundException(string message) : base(message) {}
    }
}