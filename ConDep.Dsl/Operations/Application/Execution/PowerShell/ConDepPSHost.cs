using System;
using System.Globalization;
using System.Management.Automation.Host;
using System.Reflection;
using System.Threading;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
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
}