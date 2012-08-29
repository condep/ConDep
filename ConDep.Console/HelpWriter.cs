using System.IO;
using NDesk.Options;

namespace ConDep.Console
{
    public class HelpWriter
    {
        private readonly TextWriter _writer;

        public HelpWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void PrintHelp(OptionSet optionSet)
        {
            PrintHelpUsage(optionSet, _writer);
            PrintHelpExamples(_writer);
            PrintHelpExplanations(_writer);
        }

        private static void PrintHelpUsage(OptionSet optionSet, TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("Deploy files and infrastructure to remote servers and environments");
            writer.WriteLine();
            writer.WriteLine("Usage: ConDep <assembly> <environment> [-options]");
            writer.WriteLine();
            writer.WriteLine("  <assembly>\t\tAssembly containing deployment setup");
            writer.WriteLine("  \t\t\tIf no path to assembly is specified, first current ");
            writer.WriteLine("  \t\t\tdirectory is searched and then executing directory. ");
            writer.WriteLine("  \t\t\tPrecedence is in the order specified here.");
            writer.WriteLine();
            writer.WriteLine("  <environment>\t\tEnvironment to deploy to (e.g. Dev, Test etc)");
            writer.WriteLine();
            writer.WriteLine("where options include:");
            optionSet.WriteOptionDescriptions(writer);
            writer.WriteLine();
        }

        private static void PrintHelpExamples(TextWriter writer)
        {
            writer.WriteLine("Examples:");
            writer.WriteLine("\t(1) ConDep.exe MyAssembly.dll Dev");
            writer.WriteLine();
            writer.WriteLine("\t(2) ConDep.exe MyAssembly.dll Dev -s MyWebServer");
            writer.WriteLine();
            writer.WriteLine("\t(3) ConDep.exe MyAssembly.dll Dev -s MyWebServer -c MyContext");
            writer.WriteLine();
            writer.WriteLine("\t(4) ConDep.exe MyAssembly.dll Dev -c MyContext");
            writer.WriteLine();
            writer.WriteLine("\t(5) ConDep.exe MyAssembly.dll Dev -c MyContext -deployOnly");
            writer.WriteLine();
            writer.WriteLine("\t(6) ConDep.exe MyAssembly.dll Dev -c MyContext -infraOnly");
            writer.WriteLine();
            writer.WriteLine("\t(7) ConDep.exe MyAssembly.dll Dev -infraOnly");
            writer.WriteLine();
            writer.WriteLine("\t(8) ConDep.exe MyAssembly.dll Dev -deployOnly");
            writer.WriteLine();
        }

        private static void PrintHelpExplanations(TextWriter writer)
        {
            writer.WriteLine("Explanations:");
            writer.WriteLine("\t1 - Deploy setup found in MyAssembly.dll to all servers in");
            writer.WriteLine("\t    the Dev environment.");
            writer.WriteLine();
            writer.WriteLine("\t2 - Deploy setup found in MyAssembly.dll do the Dev environment, ");
            writer.WriteLine("\t    but only to the server MyWebServer.");
            writer.WriteLine();
            writer.WriteLine("\t3 - Same as above, except only deploys the configuration defined in");
            writer.WriteLine("\t    the MyContext context.");
            writer.WriteLine();
            writer.WriteLine("\t4 - Deploy the context MyContext to all servers in the Dev environment.");
            writer.WriteLine();
            writer.WriteLine("\t5 - Same as above, except only deployment is executed and no infrastructure.");
            writer.WriteLine();
            writer.WriteLine("\t6 - Same as above, except only infrastructure is executed and no deployment.");
            writer.WriteLine();
            writer.WriteLine("\t7 - Deploy infrastructure setup only.");
            writer.WriteLine();
            writer.WriteLine("\t8 - Will only deploy deployment specific setup and not infrastrucutre.");
        }
    }
}