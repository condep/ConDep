using System.IO;
using NDesk.Options;

namespace ConDep.Console.Server
{
    public class CmdServerParser : CmdBaseParser<ConDepServerOptions>
    {
        public CmdServerParser(string[] args) : base(args)
        {
        }

        public override OptionSet OptionSet
        {
            get { return null; }
        }

        public override ConDepServerOptions Parse()
        {
            return new ConDepServerOptions();
        }

        public override void WriteOptionsHelp(TextWriter writer)
        {
        }
    }

    public class ConDepServerOptions
    {
    }
}