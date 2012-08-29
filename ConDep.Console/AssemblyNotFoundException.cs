using System;

namespace ConDep.Console
{
    public class AssemblyNotFoundException : Exception
    {
        public AssemblyNotFoundException(string message) : base(message) {}
    }
}