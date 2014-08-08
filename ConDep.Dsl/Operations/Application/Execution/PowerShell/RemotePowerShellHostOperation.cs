﻿using System.IO;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Remote;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class RemotePowerShellHostOperation : RemoteOperation
    {
        private readonly string _cmd;
        private readonly FileInfo _scriptFile;
        private readonly PowerShellOptions.PowerShellOptionValues _values;

        public RemotePowerShellHostOperation(string cmd, PowerShellOptions.PowerShellOptionValues values = null)
        {
            _cmd = cmd;
            _values = values;
        }

        public RemotePowerShellHostOperation(FileInfo scriptFile, PowerShellOptions.PowerShellOptionValues values = null)
        {
            _scriptFile = scriptFile;
            _values = values;
        }

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings, CancellationToken token)
        {
            var psExec = new PowerShellExecutor(server);
            if (_values != null)
            {
                if (_values.RequireRemoteLib)
                {
                    psExec.LoadConDepDotNetLibrary = true;
                }

                if (_values.SkipLoadingConDepModule)
                {
                    psExec.LoadConDepModule = false;
                }
            }

            psExec.Execute(_cmd);
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
}