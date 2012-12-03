using System;
using System.IO;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeployProviders.PowerShell;
using ConDep.Dsl.WebDeployProviders.RunCmd;

namespace ConDep.Dsl.Experimental.Application
{
    public class RemoteExecutor : IOfferRemoteExecution
    {
        private readonly IManageRemoteSequence _remoteSequence;
        private readonly IOperateWebDeploy _webDeploy;
        private readonly ILogForConDep _logger;

        public RemoteExecutor(IManageRemoteSequence remoteSequence, IOperateWebDeploy webDeploy, ILogForConDep logger, RemoteServerOffer remoteServerOffer)
        {
            _remoteSequence = remoteSequence;
            _webDeploy = webDeploy;
            _logger = logger;
        }

        public IOfferRemoteExecution DosCommand(string cmd)
        {
            return DosCommand(cmd, false);
        }

        public IOfferRemoteExecution DosCommand(string cmd, bool continueOnError)
        {
            var runCmdProvider = new RunCmdProvider(cmd, continueOnError);
            _remoteSequence.Add(new RemoteOperation(runCmdProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution DosCommand(string cmd, bool continueOnError, Action<RunCmdOptions> runCmdOptions)
        {
            var runCmdProvider = new RunCmdProvider(cmd, continueOnError);
            runCmdOptions(new RunCmdOptions(runCmdProvider));
            _remoteSequence.Add(new RemoteOperation(runCmdProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(string commandOrScript)
        {
            var psProvider = new PowerShellProvider(commandOrScript);
            _remoteSequence.Add(new RemoteOperation(psProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile)
        {
            var psProvider = new PowerShellProvider(scriptFile);
            _remoteSequence.Add(new RemoteOperation(psProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(string commandOrScript, Action<PowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellProvider(commandOrScript);
            powerShellOptions(new PowerShellOptions(psProvider));
            _remoteSequence.Add(new RemoteOperation(psProvider, _logger, _webDeploy));
            return this;
        }

        public IOfferRemoteExecution PowerShell(FileInfo scriptFile, Action<PowerShellOptions> powerShellOptions)
        {
            var psProvider = new PowerShellProvider(scriptFile);
            powerShellOptions(new PowerShellOptions(psProvider));
            _remoteSequence.Add(new RemoteOperation(psProvider, _logger, _webDeploy));
            return this;
        }


        public IManageRemoteSequence Sequence
        {
            get { return _remoteSequence; }
        }
    }
}