using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;
using TinyIoC;

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
            if(_envConfig.LoadBalancer != null)
            {
                container.Register(new LoadBalancerLookup(_envConfig.LoadBalancer).GetLoadBalancer());
                container.Register(_envConfig.LoadBalancer);
            }
            else
            {
                container.Register<ILoadBalance, DefaultLoadBalancer>();
            }
            //container.Register<ISetupWebDeploy, WebDeploySetup>().AsMultiInstance();
            //container.Register<ISetupConDep, ConDepSetup>().AsMultiInstance();

            container.Register<IOfferLocalOperations, LocalOperationsBuilder>().AsMultiInstance();
            container.Register<IOfferRemoteDeployment, RemoteDeploymentBuilder>().AsMultiInstance();
            container.Register<IOfferRemoteExecution, RemoteExecutionBuilder>().AsMultiInstance();

            container.Register<IManageExecutionSequence, ExecutionSequenceManager>(_execSeq);

            container.Register<IManageRemoteSequence, RemoteSequenceManager>().AsMultiInstance();
            container.Register<IManageLocalSequence, LocalSequenceManager>().AsMultiInstance();
            
            container.Register<IOfferRemoteCertDeployment, RemoteCertDeploymentBuilder>().AsMultiInstance();
            //container.Register<RemoteServerOffer>((c,e) => new RemoteServerOffer(execSeq, servers));//().UsingConstructor(() => new RemoteServerOffer(execSeq, _envConfig.Servers));
            container.Register<IOfferRemoteOperations>(CreateRemoteServers);
            //container.Register<IOfferServerRemoting, RemoteServerOffer>().AsMultiInstance();

            container.Register<IOperateWebDeploy, WebDeployOperator>();
            container.Register(new Logger().Resolve());
        }

        private RemoteOperationsBuilder CreateRemoteServers(TinyIoCContainer container, NamedParameterOverloads overloads)
        {
            var webDeploy = container.Resolve<IOperateWebDeploy>();
            return new RemoteOperationsBuilder(_execSeq, _envConfig.Servers, webDeploy);
        }

        public static void Bootstrap(ConDepConfig envSettings)
        {
            new IoCBootstrapper(envSettings).BootstrapTinyIoC();
        }
    }
}