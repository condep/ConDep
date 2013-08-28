using System.IO;
using NDesk.Options;

namespace ConDep.Console
{
    public class CmdDeployHelpWriter : CmdHelpWriter
    {
        public CmdDeployHelpWriter(TextWriter writer) : base(writer)
        {
        }

        public override void WriteHelp(OptionSet optionSet)
        {
            PrintCopyrightMessage();

            var help = @"
Deploy files and infrastructure to remote servers and environments

Usage: ConDep deploy <assembly> <environment> <<application> | --deployAllApps> [-options]

  <assembly>        Assembly containing deployment setup
                    If no path to assembly is specified, first current 
                    directory is searched and then executing directory. 
                    Precedence is in the order specified here.

  <environment>     Environment to deploy to (e.g. Dev, Test etc)

  <application>     Application to deploy.
  or
  --deployAllApps   ...to deploy all applications.

where options include:

";
            _writer.Write(help);
            optionSet.WriteOptionDescriptions(_writer);
        }
    }
}