using System;
using System.IO;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;

namespace ConDep.Dsl.Builders
{
    public interface IOfferRemoteExecution
    {
        IOfferRemoteExecution DosCommand(string cmd);
        IOfferRemoteExecution DosCommand(string cmd, Action<RunCmdOptions> runCmdOptions);

        IOfferRemoteExecution PowerShell(string commandOrScript);
        IOfferRemoteExecution PowerShell(FileInfo scriptFile);
        
        IOfferRemoteExecution PowerShell(string commandOrScript, Action<PowerShellOptions> powerShellOptions);
        IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions);
    }
}