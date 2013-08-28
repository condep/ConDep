using System;

namespace ConDep.Console
{
    public class ConDepCmdParseException : Exception
    {
        public ConDepCmdParseException(string message) : base(message) { }

        public ConDepCmdParseException(string message, Exception inner) : base(message, inner) {}
    }
}