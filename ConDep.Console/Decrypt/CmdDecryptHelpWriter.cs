using System.IO;
using NDesk.Options;

namespace ConDep.Console.Decrypt
{
    public class CmdDecryptHelpWriter : CmdHelpWriter
    {
        public CmdDecryptHelpWriter(TextWriter writer) : base(writer)
        {
            
        }

        public override void WriteHelp(OptionSet optionSet)
        {
            PrintCopyrightMessage();

            var help = @"
Decrypt sensitive data like passwords in encrypted json config files

Usage: ConDep decrypt <key> [-options]

  <key>     Key to use for decryption.
 
where options include:

";
            _writer.Write(help);

            optionSet.WriteOptionDescriptions(_writer);
            _writer.WriteLine();
        }

    }
}