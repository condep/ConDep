using System.IO;
using NDesk.Options;

namespace ConDep.Console
{
    internal class HelpWriter
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
        }

        private static void PrintHelpUsage(OptionSet optionSet, TextWriter writer)
        {
            var help = @"
Deploy files and infrastructure to remote servers and environments

Usage: ConDep <assembly> <environment> <<application> | --deployAllApps> [-options]

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
            writer.Write(help);
            optionSet.WriteOptionDescriptions(writer);
        }

        private static void PrintHelpExamples(TextWriter writer)
        {
            var help = @"
Examples:

    Deploy MyWebApp to Dev environment:

        ConDep.exe MyAssembly.dll Dev MyWebApp

    Deploy all applications to Dev environment:

        ConDep.exe MyAssembly.dll Dev --deployAllApps

    Deploy only the infrastructure defined for MyWebApp:        

        ConDep.exe MyAssembly.dll Dev MyWebApp --infraOnly

    Deploy MyWebApp to Dev environment, but don't do infrastructure:

        ConDep.exe MyAssembly.dll Dev MyWebApp --deployOnly

    Deploy MyNonCriticalApp to Dev environment, not using load balancer to
    take servers offline/online:

        ConDep.exe MyAssembly.dll Dev MyNonCriticalApp --bypassLB

    Deploy MyWebApp to Dev environment, but only on the first server 
    (or marked server i json-config). If a load balancer is configured, 
    the server will stay offline in the load balancer. The second 
    command continues where the first one left off. This allows for 
    manuel testing on one server, before deploying to all servers:

        ConDep.exe MyAssembly.dll Dev MyWebApp --stopAfterMarkedServer
        ConDep.exe MyAssembly.dll Dev MyWebApp --continueAfterMarkedServer

";
            writer.Write(help);
        }
    }
}