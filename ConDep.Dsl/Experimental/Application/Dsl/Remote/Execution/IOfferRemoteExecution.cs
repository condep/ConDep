using System;
using System.IO;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeployProviders.PowerShell;
using ConDep.Dsl.WebDeployProviders.RunCmd;

namespace ConDep.Dsl.Experimental.Application
{
    public interface IOfferRemoteExecution
    {
        IOfferRemoteExecution DosCommand(string cmd);
        IOfferRemoteExecution DosCommand(string cmd, bool continueOnError);
        IOfferRemoteExecution DosCommand(string cmd, bool continueOnError, Action<RunCmdOptions> runCmdOptions);
        IOfferRemoteExecution PowerShell(string commandOrScript);
        IOfferRemoteExecution PowerShell(FileInfo scriptFile);
        IOfferRemoteExecution PowerShell(string commandOrScript, Action<PowerShellOptions> powerShellOptions);
        IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions);
    }
}