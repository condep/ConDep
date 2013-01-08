using System;
using System.IO;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.Builders
{
    public class RemoteExecutionBuilder : IOfferRemoteExecution
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
            return DosCommand(cmd, false);
        }

        public IOfferRemoteExecution DosCommand(string cmd, bool continueOnError)
        {
            var runCmdProvider = new RunCmdProvider(cmd, continueOnError);
            _remoteSequence.Add(new RemoteWebDeployOperation(runCmdProvider, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution DosCommand(string cmd, bool continueOnError, Action<RunCmdOptions> runCmdOptions)
        {
            var runCmdProvider = new RunCmdProvider(cmd, continueOnError);
            runCmdOptions(new RunCmdOptions(runCmdProvider));
            _remoteSequence.Add(new RemoteWebDeployOperation(runCmdProvider, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(string commandOrScript)
        {
            var psProvider = new PowerShellProvider(commandOrScript);
            psProvider.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(psProvider), _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile)
        {
            var psProvider = new PowerShellProvider(scriptFile);
            psProvider.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(psProvider), _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(string commandOrScript, Action<PowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellProvider(commandOrScript);
            powerShellOptions(new PowerShellOptions(psProvider));
            psProvider.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(psProvider), _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellProvider(scriptFile);
            powerShellOptions(new PowerShellOptions(psProvider));
            psProvider.Configure(new RemoteCompositeBuilder(_remoteSequence.NewCompositeSequence(psProvider), _webDeploy));
            return this;
        }


        //public IManageRemoteSequence Sequence
        //{
        //    get { return _remoteSequence; }
        //}
    }
}