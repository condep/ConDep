using System.Collections.Generic;
using ConDep.Dsl.Experimental.Application;
using ConDep.Dsl.Experimental.Application.Deployment;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.Experimental.Core.Impl;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;
using TinyIoC;
using System.Linq;

namespace ConDep.Dsl
{
    internal class IoCBootstrapper
    {
        private readonly ConDepConfig _envConfig;
        private ExecutionSequenceManager _execSeq;

        private IoCBootstrapper(ConDepConfig envConfig)
        {
            _envConfig = envConfig;
            _execSeq = new ExecutionSequenceManager();
        }

        public void BootstrapTinyIoC()
        {
            var container = TinyIoCContainer.Current;

            container.Register(_envConfig);
            container.Register<ISetupWebDeploy, WebDeploySetup>().AsMultiInstance();
            container.Register<ISetupConDep, ConDepSetup>().AsMultiInstance();
            container.Register(_envConfig.LoadBalancer);

            container.Register<IOfferApplicationOps, ApplicationOps>().AsMultiInstance();
            container.Register<IOfferRemoteDeployment, RemoteDeployment>().AsMultiInstance();
            container.Register<IOfferRemoteExecution, RemoteExecutor>().AsMultiInstance();

            container.Register<IManageExecutionSequence, ExecutionSequenceManager>(_execSeq);

            container.Register<IManageRemoteSequence, RemoteSequenceManager>().AsMultiInstance();
            container.Register<IManageLocalSequence, LocalSequenceManager>().AsMultiInstance();
            
            container.Register<IOfferRemoteSslOperations, SslCertificateDeployer>().AsMultiInstance();
            //container.Register<RemoteServerOffer>((c,e) => new RemoteServerOffer(execSeq, servers));//().UsingConstructor(() => new RemoteServerOffer(execSeq, _envConfig.Servers));
            container.Register<IOfferRemoteOptions>(CreateRemoteServers);
            //container.Register<IOfferServerRemoting, RemoteServerOffer>().AsMultiInstance();

            container.Register<IOperateWebDeploy, WebDeployOperator>();
            container.Register(new Logger().Resolve());
        }

        private RemoteServerOffer CreateRemoteServers(TinyIoCContainer container, NamedParameterOverloads overloads)
        {
            var webDeploy = container.Resolve<IOperateWebDeploy>();
            var logger = container.Resolve<ILogForConDep>();
            return new RemoteServerOffer(_execSeq, _envConfig.Servers, webDeploy, logger);
        }

        public static void Bootstrap(ConDepConfig envSettings)
        {
            new IoCBootstrapper(envSettings).BootstrapTinyIoC();
        }
    }
}