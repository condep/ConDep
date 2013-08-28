using System.IO;
using NDesk.Options;

namespace ConDep.Console
{
    public abstract class CmdBaseParser<T>
    {
        protected readonly string[] _args;

        protected CmdBaseParser(string[] args)
        {
            _args = args;
        }

        public abstract OptionSet OptionSet { get; }

        public abstract T Parse();
        public abstract void WriteOptionsHelp(TextWriter writer);
    }
}