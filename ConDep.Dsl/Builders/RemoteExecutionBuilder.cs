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
        private readonly IHandleWebDeploy _webDeploy;

        public RemoteExecutionBuilder(IManageRemoteSequence remoteSequence, IHandleWebDeploy webDeploy)
        {
            _remoteSequence = remoteSequence;
            _webDeploy = webDeploy;
        }

        public IOfferRemoteExecution DosCommand(string cmd)
        {
            var runCmdProvider = new RunCmdProvider(cmd);
            AddOperation(runCmdProvider);
            return this;
        }

        public IOfferRemoteExecution DosCommand(string cmd, Action<RunCmdOptions> runCmdOptions)
        {
            var runCmdProvider = new RunCmdProvider(cmd);
            runCmdOptions(new RunCmdOptions(runCmdProvider));
            AddOperation(runCmdProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(string commandOrScript)
        {
            var psProvider = new PowerShellProvider(commandOrScript);
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile)
        {
            var psProvider = new PowerShellProvider(scriptFile);
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(string commandOrScript, Action<PowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellProvider(commandOrScript);
            powerShellOptions(new PowerShellOptions(psProvider));
            AddOperation(psProvider);
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellProvider(scriptFile);
            powerShellOptions(new PowerShellOptions(psProvider));
            AddOperation(psProvider);
            return this;
        }


        //public IManageRemoteSequence Sequence
        //{
        //    get { return _remoteSequence; }
        //}
        public void AddOperation(RemoteCompositeOperation operation)
        {
            operation.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(operation), _webDeploy));
        }

        public void AddOperation(WebDeployProviderBase provider)
        {
            _remoteSequence.Add(new RemoteWebDeployOperation(provider, _webDeploy));
        }
    }
}