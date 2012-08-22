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
            writer.WriteLine("\t(3) ConDep.exe MyAssembly.dll Dev -s MyWebServer -a MyWebApp");
            writer.WriteLine();
            writer.WriteLine("\t(4) ConDep.exe MyAssembly.dll Dev -a MyWebApp");
            writer.WriteLine();
            writer.WriteLine("\t(5) ConDep.exe MyAssembly.dll Dev -a MyWebApp -d");
            writer.WriteLine();
            writer.WriteLine("\t(6) ConDep.exe MyAssembly.dll Dev -a MyWebApp -i");
            writer.WriteLine();
            writer.WriteLine("\t(7) ConDep.exe MyAssembly.dll Dev -i");
            writer.WriteLine();
            writer.WriteLine("\t(8) ConDep.exe MyAssembly.dll Dev -d");
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
            writer.WriteLine("\t3 - Same as above, except only deploys the application MyWebApp.");
            writer.WriteLine("\t    This also meens no infrastructure is deployed (-do option).");
            writer.WriteLine();
            writer.WriteLine("\t4 - Deploy the application MyWebApp to all servers in the");
            writer.WriteLine("\t    Dev environment. (here the -do option is implisit).");
            writer.WriteLine();
            writer.WriteLine("\t5 - Same as above, only here -do is explicitly set.");
            writer.WriteLine();
            writer.WriteLine("\t6 - This will result in an error, cause you cannot deploy");
            writer.WriteLine("\t    an application with the infrastructure only option set.");
            writer.WriteLine();
            writer.WriteLine("\t7 - Deploy infrastructure setup only.");
            writer.WriteLine();
            writer.WriteLine("\t8 - Will only deploy deployment specific setup and");
            writer.WriteLine("\t    not infrastrucutre.");
        }
    }
}