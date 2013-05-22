using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Remote
{
    internal class ConDepPSHostUI : PSHostUserInterface
    {
        private ConDepPsHostRawUI _rawUi;

        public ConDepPSHostUI()
        {
            _rawUi = new ConDepPsHostRawUI();
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }

        public override SecureString ReadLineAsSecureString()
        {
            throw new NotImplementedException();
        }

        public override void Write(string value)
        {
            if (value == "\n") return;
            Logger.Info(value);
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            if (value == "\n") return;
            Logger.Info(value);
        }

        public override void WriteLine(string value)
        {
            Logger.Info(value);
        }

        public override void WriteErrorLine(string value)
        {
            Logger.Error(value);
        }

        public override void WriteDebugLine(string message)
        {
            Logger.Verbose(message);
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            //
        }

        public override void WriteVerboseLine(string message)
        {
            Logger.Verbose(message);
        }

        public override void WriteWarningLine(string message)
        {
            Logger.Warn(message);
        }

        public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
        {
            return null;

            //Dictionary<string, PSObject> ret = new Dictionary<string, PSObject>();

            //foreach (FieldDescription desc in descriptions)
            //{
            //    if (this.promptInput.Count != 0)
            //    {
            //        ret[desc.Name] = new PSObject(this.promptInput[desc.Name] + "\r\n");
            //    }
            //    else if (this.lineInput != null && this.currentLineInput >= 0 && this.currentLineInput < this.lineInput.Length)
            //    {
            //        ret[desc.Name] = new PSObject(this.lineInput[this.currentLineInput++] + "\r\n");
            //    }
            //    else
            //    {
            //        if (desc.DefaultValue == null)
            //        {
            //            ret[desc.Name] = new PSObject("\r\n");
            //        }
            //        else
            //        {
            //            ret[desc.Name] = new PSObject(desc.DefaultValue);
            //        }
            //    }
            //}

            //return ret;
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        {
            throw new NotImplementedException();
        }

        public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
        {
            return defaultChoice;
        }

        public override PSHostRawUserInterface RawUI
        {
            get { return _rawUi; }
        }
    }
}