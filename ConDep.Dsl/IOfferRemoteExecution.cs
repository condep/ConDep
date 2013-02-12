using System;
using System.IO;

namespace ConDep.Dsl
{
    public interface IOfferRemoteExecution
    {
        /// <summary>
        /// Will execute a DOS command using cmd.exe on remote server.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        IOfferRemoteExecution DosCommand(string cmd);

        /// <summary>
        /// Will execute a DOS command using cmd.exe on remote server with provided options.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="runCmdOptions"></param>
        /// <returns></returns>
        IOfferRemoteExecution DosCommand(string cmd, Action<IOfferRunCmdOptions> runCmdOptions);

        /// <summary>
        /// Will execute a PowerShell command on remote server.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IOfferRemoteExecution PowerShell(string command);

        /// <summary>
        /// Will execute a PowerShell command on remote server with provided options.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="powerShellOptions"></param>
        /// <returns></returns>
        IOfferRemoteExecution PowerShell(string command, Action<IOfferPowerShellOptions> powerShellOptions);

        /// <summary>
        /// Will deploy and execute provided PowerShell script on remote server.
        /// </summary>
        /// <param name="scriptFile"></param>
        /// <returns></returns>
        IOfferRemoteExecution PowerShell(FileInfo scriptFile);

        /// <summary>
        /// Will deploy and execute provided PowerShell script on remote server with provided options.
        /// </summary>
        /// <param name="scriptFile"></param>
        /// <param name="powerShellOptions"></param>
        /// <returns></returns>
        IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<IOfferPowerShellOptions> powerShellOptions);
    }
}