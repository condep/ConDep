using System;
using System.IO;
using ConDep.Dsl.Operations;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

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
            //var runCmdProvider = new RunCmdProvider(cmd);
            //AddOperation(runCmdProvider);
            var runCmdOperation = new RunCmdPsOperation(cmd);
            AddOperation(runCmdOperation);
            return this;
        }

        public IOfferRemoteExecution DosCommand(string cmd, Action<IOfferRunCmdOptions> runCmdOptions)
        {
            //var runCmdProvider = new RunCmdProvider(cmd);
            //runCmdOptions(new RunCmdOptions(runCmdProvider));
            //AddOperation(runCmdProvider);
            var runCmdOperation = new RunCmdPsOperation(cmd);
            AddOperation(runCmdOperation);
            return this;
        }

        public IOfferRemoteExecution PowerShell(string command)
        {
            //var psProvider = new PowerShellOperation(command);
            var psProvider = new RemotePowerShellHostOperation(command);
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile)
        {
            var psProvider = new PowerShellOperation(scriptFile);
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(string command, Action<IOfferPowerShellOptions> powerShellOptions)
        {
            //var psProvider = new PowerShellOperation(command);
            var psProvider = new RemotePowerShellHostOperation(command);
            //powerShellOptions(new PowerShellOptions(psProvider));
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<IOfferPowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellOperation(scriptFile);
            powerShellOptions(new PowerShellOptions(psProvider));
            AddOperation(psProvider);
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