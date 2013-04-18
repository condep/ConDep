using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Security;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using ConDep.Dsl.Operations.Application.Local;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
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

    internal class ConDepPsHostRawUI : PSHostRawUserInterface
    {
        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException();
        }

        public override void FlushInputBuffer()
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override ConsoleColor ForegroundColor { get { return Logger.ForegroundColor; } set { Logger.ForegroundColor = value; } }
        public override ConsoleColor BackgroundColor { get { return Logger.BackgroundColor; } set { Logger.BackgroundColor = value; } }
        public override Coordinates CursorPosition { get; set; }
        public override Coordinates WindowPosition { get; set; }
        public override int CursorSize { get; set; }
        public override Size BufferSize { get {return new Size(9999, 9999);} set {} }
        public override Size WindowSize { get; set; }

        public override Size MaxWindowSize
        {
            get { return new Size(5000, 5000); }
        }

        public override Size MaxPhysicalWindowSize
        {
            get { return new Size(5000, 5000); }
        }

        public override bool KeyAvailable
        {
            get { return false; }
        }

        public override string WindowTitle { get; set; }
    }

    internal class ConDepPSHost : PSHost
    {
        private readonly Guid _hostId;
        private ConDepPSHostUI _hostUi;

        public ConDepPSHost()
        {
            _hostId = Guid.NewGuid();
            _hostUi = new ConDepPSHostUI();
        }

        public override void SetShouldExit(int exitCode)
        {
            throw new ConDepUntrappedExitCodeException(exitCode.ToString());
            //this.program.ShouldExit = true;
            //this.program.ExitCode = exitCode;
        }

        public override void EnterNestedPrompt()
        {
            //throw new NotImplementedException();
        }

        public override void ExitNestedPrompt()
        {
            //throw new NotImplementedException();
        }

        public override void NotifyBeginApplication()
        {
        }

        public override void NotifyEndApplication()
        {
        }

        public override string Name
        {
            get { return "ConDepPSHost"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override Guid InstanceId
        {
            get { return _hostId; }
        }

        public override PSHostUserInterface UI
        {
            get { return _hostUi; }
        }

        public override CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        public override CultureInfo CurrentUICulture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }

    }

    public class RemotePowerShellHostOperation : RemoteOperation
    {
        private const string SHELL_URI = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        private readonly string _cmd;
        private bool _stillExecuting;
        private Pipeline _pipeline;

        public RemotePowerShellHostOperation(string cmd)
        {
            _cmd = cmd;
        }

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            var host = new ConDepPSHost();

            var remoteCredential = new PSCredential(server.DeploymentUser.UserName, server.DeploymentUser.PasswordAsSecString);
            var connectionInfo = new WSManConnectionInfo(false, server.Name, 5985, "/wsman", SHELL_URI,
                                                         remoteCredential);
                                     //{AuthenticationMechanism = AuthenticationMechanism.Negotiate, SkipCACheck = true, SkipCNCheck = true, SkipRevocationCheck = true};

            using (var runspace = RunspaceFactory.CreateRunspace(host, connectionInfo))
            {
                _stillExecuting = true;
                runspace.Open();

                var conDepModule = string.Format(@"Import-Module $env:windir\temp\ConDep\{0}\PSScripts\ConDep;", ConDepGlobals.ExecId);
                var psCmd = string.Format(@"set-executionpolicy remotesigned -force; {0} {1};", conDepModule, _cmd);

                Logger.Info("Executing PowerShell command: " + _cmd);
                var ps = System.Management.Automation.PowerShell.Create().AddScript(psCmd);
                ps.Runspace = runspace;
                var result = ps.Invoke();

                foreach(var psObject in result)
                {
                    Logger.Info(psObject.ToString());
                }
                    
                //_pipeline = runspace.CreatePipeline(psCmd);
                //_pipeline.Output.DataReady += OutputOnDataReady;
                //_pipeline.Error.DataReady += ErrorOnDataReady;
                //_pipeline.StateChanged += PipelineOnStateChanged;

                //try
                //{
                //    _pipeline.InvokeAsync();

                //    while (_stillExecuting)
                //    {
                //        Thread.Sleep(100);
                //    }

                //    if(_pipeline.PipelineStateInfo.State == PipelineState.Failed)
                //    {
                //        var data = _pipeline.Error.NonBlockingRead();
                //        foreach(var obj in data)
                //        {
                //            Logger.Error(obj.ToString());
                //        }
                //    }
                //}
                //catch(Exception ex)
                //{
                //    Logger.Error("Exception: ", ex);
                //}
            }
        }

        private void PipelineOnStateChanged(object sender, PipelineStateEventArgs pipelineStateEventArgs)
        {
            if(pipelineStateEventArgs.PipelineStateInfo.State == PipelineState.Failed || 
                pipelineStateEventArgs.PipelineStateInfo.State == PipelineState.Stopped ||
                pipelineStateEventArgs.PipelineStateInfo.State == PipelineState.Completed)
            {
                _stillExecuting = false;
            }
        }

        private void ErrorOnDataReady(object sender, EventArgs eventArgs)
        {
            var data = _pipeline.Error.NonBlockingRead();
            if(data != null)
            {
                if(data.Count > 0)
                {
                    foreach(var psObject in data)
                    {
                        Logger.Error(psObject.ToString());
                    }
                }
            }
        }

        private void OutputOnDataReady(object sender, EventArgs eventArgs)
        {
            var data = _pipeline.Output.NonBlockingRead();
            if(data != null)
            {
                if (data.Count > 0)
                {
                    foreach (var obj in data)
                    {
                        Logger.Info(obj.ToString());
                    }
                }
            }
        }

        public override string Name
        {
            get { return "Remote PowerShell"; }
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }

    public abstract class RemoteOperation : IOperateRemote
    {
        public abstract void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings);
        public abstract string Name { get; }
        public abstract bool IsValid(Notification notification);

    }
}