using System.IO;
using NDesk.Options;

namespace ConDep.Console.Server
{
    public class CmdServerHelpWriter : CmdHelpWriter
    {
        public CmdServerHelpWriter(TextWriter writer) : base(writer)
        {
        }

        public override void WriteHelp(OptionSet optionSet)
        {
            PrintCopyrightMessage();

            var help = @"
Interact with a ConDep Server

Usage: ConDep Server <command>

where available commands are:

  Upload            Upload environment configurations (*.env.json), 
                    deployment artifacts (*.dll) or deployment pipelines (*.pipe.json)

  Execute           Execute a deployment artifact or deployment pipeline

  List              List content from ConDep Server. Available content are configurations,
                    pipelines, deployment artifacts and logs.

  Show              Show content from ConDep Server. Available content are configurations,
                    pipelines, and logs.

  Help <command>    Show help about any of the above commands

  For more information on the individual commands, type ConDep Server Help <command>
";
            _writer.Write(help);
        }
    }
}