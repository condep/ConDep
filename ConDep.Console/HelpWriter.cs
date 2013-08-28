using System.Diagnostics;
using System.IO;
using System.Reflection;
using NDesk.Options;

namespace ConDep.Console
{
    public interface IOutputHelp
    {
        
    }


    public class HelpWriter
    {
        private readonly TextWriter _writer;

        public HelpWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void PrintHelp(OptionSet optionSet)
        {
            PrintCopyrightMessage(_writer);
            PrintHelpUsage(optionSet, _writer);
            PrintHelpExamples(_writer);
        }

        public void PrintHelp(string helpCommand, OptionSet optionSet = null)
        {
            PrintCopyrightMessage(_writer);
            PrintHelpUsage(optionSet, _writer);
            PrintHelpExamples(_writer);
        }

        private static void PrintCopyrightMessage(TextWriter writer)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            writer.WriteLine("ConDep Version {0}", versionInfo.ProductVersion.Substring(0, versionInfo.ProductVersion.LastIndexOf(".")));
            writer.WriteLine("Copyright (c) Jon Arild Torresdal");
        }

        public void PrintRootHelp()
        {
            PrintCopyrightMessage(_writer);

            var help = @"
Deploy files and infrastructure to remote servers and environments

Usage: ConDep <command> <options>

Available commands:
    Deploy      Deploy ConDep definitions to one or more servers using specified json-config
    Encrypt     Encrypt sensitive data, like passwords, in json-config files
    Decrypt     Decrypt encrypted data in json-config files

For help on individual commands, type condep.exe help <command>.

";
            _writer.Write(help);
        }

        private static void PrintHelpUsage(OptionSet optionSet, TextWriter writer)
        {
            var help = @"
Deploy files and infrastructure to remote servers and environments

Usage: ConDep <assembly> <environment> <<application> | --deployAllApps> [-options]

Usage: ConDep --installWebQ [server]

  <assembly>        Assembly containing deployment setup
                    If no path to assembly is specified, first current 
                    directory is searched and then executing directory. 
                    Precedence is in the order specified here.

  <environment>     Environment to deploy to (e.g. Dev, Test etc)

  <application>     Application to deploy.
  or
  --deployAllApps   ...to deploy all applications.

  --installWebQ     Installs ConDep's WebQ Services on local computer or
                    on remote server [server]

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

    Install ConDep's WebQ Service on remote server:
        ConDep.exe --installWebQ someServer

";
            writer.Write(help);
        }
    }
}