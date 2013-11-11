using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ConDep.Console.Decrypt;
using ConDep.Console.Deploy;
using ConDep.Console.Encrypt;
using ConDep.Console.Server;
using NDesk.Options;

namespace ConDep.Console
{
    public class CmdHelpWriter
    {
        protected readonly TextWriter _writer;

        public CmdHelpWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public virtual void WriteHelp(Exception ex, OptionSet optionSet)
        {
            WriteException(ex);
            WriteHelp();
        }

        public virtual void WriteHelp(OptionSet optionSet)
        {
            
        }

        public virtual void WriteHelp()
        {
            PrintCopyrightMessage();

            var help = @"
Deploy files and infrastructure to remote servers and environments

Usage: ConDep <command> <options>

Available commands:
    Deploy              Deploy files and infrastructure configurations to one or more servers
    Encrypt             Encrypt sensitive data, like passwords, in json-config files
    Decrypt             Decrypt encrypted data in json-config files
    Server              Enable interaction with a ConDep Server
    Help <command>      Display help for individual help commands
";
            _writer.Write(help);
        }

        public void PrintCopyrightMessage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            const int versionAreaLength = 29;
            var version = versionInfo.ProductVersion.Substring(0, versionInfo.ProductVersion.LastIndexOf("."));
            var versionText = string.Format("Version {0} ", version);
            var versionWhitespace = string.Join(" ", new string[versionAreaLength - (versionText.Length - 1)]);

            //Logger.Info(string.Format("ConDep Version {0}", version));
            _writer.Write(@"
Copyright (c) Jon Arild Torresdal
   ____            ____             
  / ___|___  _ __ |  _ \  ___ _ __  
 | |   / _ \| '_ \| | | |/ _ \ '_ \ 
 | |__| (_) | | | | |_| |  __/ |_) |
  \____\___/|_| |_|____/ \___| .__/ 
" + versionWhitespace + versionText + "|_|\n\n");
        }

        public void WriteException(Exception ex)
        {
            _writer.WriteLine(ex.Message);
            _writer.Write(ex.StackTrace);
            _writer.WriteLine();
        }

        public void WriteHelpForCommand(ConDepCommand command)
        {
            IHandleConDepCommands commandHandler;
            switch (command)
            {
                case ConDepCommand.Deploy:
                    commandHandler = new CmdDeployHandler(new string[0]);
                    break;
                case ConDepCommand.Encrypt:
                    commandHandler = new CmdEncryptHandler(new string[0]);
                    break;
                case ConDepCommand.Decrypt:
                    commandHandler = new CmdDecryptHandler(new string[0]);
                    break;
                case ConDepCommand.Server:
                    commandHandler = new CmdServerHandler(new string[0]);
                    break;
                default:
                    commandHandler = null;
                    break;
            }

            if (commandHandler == null)
                WriteHelp();
            else
                commandHandler.WriteHelp();

        }
    }
}