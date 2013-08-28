using System.IO;
using NDesk.Options;

namespace ConDep.Console.Encrypt
{
    public class CmdEncryptHelpWriter : CmdHelpWriter
    {
        public CmdEncryptHelpWriter(TextWriter writer) : base(writer)
        {
        }

        public override void WriteHelp(OptionSet optionSet)
        {
            PrintCopyrightMessage();

            var help = @"
Encrypt sensitive data like passwords in json config files

Usage: ConDep encrypt [-options]

where options include:

";
            _writer.Write(help);

            optionSet.WriteOptionDescriptions(_writer);
            _writer.WriteLine();
        }
    }
}