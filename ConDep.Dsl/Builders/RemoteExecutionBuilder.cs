using System;
using System.IO;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Builders
{
    public class RemoteExecutionBuilder : IOfferRemoteExecution, IConfigureRemoteExecution
    {
        private readonly IManageRemoteSequence _remoteSequence;

        public RemoteExecutionBuilder(IManageRemoteSequence remoteSequence)
        {
            _remoteSequence = remoteSequence;
        }

        public IOfferRemoteExecution DosCommand(string cmd)
        {
            var runCmdOperation = new RunCmdPsOperation(cmd);
            AddOperation(runCmdOperation);
            return this;
        }

        public IOfferRemoteExecution DosCommand(string cmd, Action<IOfferRunCmdOptions> runCmdOptions)
        {
            var options = new RunCmdOptions();
            runCmdOptions(options);
            var runCmdOperation = new RunCmdPsOperation(cmd, options.Values);
            AddOperation(runCmdOperation);
            return this;
        }

        public IOfferRemoteExecution PowerShell(string command)
        {
            var psProvider = new RemotePowerShellHostOperation(command);
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile)
        {
            var psProvider = new RemotePowerShellHostOperation(scriptFile);
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(string command, Action<IOfferPowerShellOptions> powerShellOptions)
        {
            var options = new PowerShellOptions();
            powerShellOptions(options);
            var operation = new RemotePowerShellHostOperation(command, options.Values);
            AddOperation(operation);
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<IOfferPowerShellOptions> powerShellOptions)
        {
            var options = new PowerShellOptions();
            powerShellOptions(options);
            var operation = new RemotePowerShellHostOperation(scriptFile, options.Values);
            AddOperation(operation);
            return this;
        }

        public void AddOperation(IOperateRemote operation)
        {
            _remoteSequence.Add(operation);
        }

        public void AddOperation(RemoteCompositeOperation operation)
        {
            operation.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(operation)));
        }
    }
}