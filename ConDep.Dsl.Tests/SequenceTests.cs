using ConDep.Dsl.Builders;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class SequenceTests
    {
        private ExecutionSequenceManager _sequenceManager;
        private SequenceTestApp _app;
        private SequenceTestInfrastructure _infra;

        [SetUp]
        public void Setup()
        {
            _sequenceManager = new ExecutionSequenceManager();
            _app = new SequenceTestApp();
            _infra = new SequenceTestInfrastructure();

            TinyIoC.TinyIoCContainer.Current.Register<ILoadBalance, DefaultLoadBalancer>();
        }

        [Test]
        public void TestThatExecutionSequenceIsValid()
        {
            var config = new ConDepConfig {EnvironmentName = "bogusEnv"};
            var server = new ServerConfig { Name = "jat-web03" };
            config.Servers = new[] { server };

            var infrastructureSequence = new InfrastructureSequence();

            var webDeploy = new WebDeployHandler();
            var infrastructureBuilder = new InfrastructureBuilder(infrastructureSequence, webDeploy);
            _infra.Configure(infrastructureBuilder);

            var local = new LocalOperationsBuilder(_sequenceManager.NewLocalSequence(), infrastructureSequence, config.Servers, webDeploy);
            _app.Configure(local, config);

            //var status = new StatusReporter();
            //_sequenceManager.Execute(status);

            var notification = new Notification();
            Assert.That(_sequenceManager.IsValid(notification));
        }
         
    }

    public class SequenceTestApp : ApplicationArtifact, IDependOnInfrastructure<SequenceTestInfrastructure>
    {
        public override void Configure(IOfferLocalOperations local, ConDepConfig config)
        {
            local.ExecuteWebRequest("GET", "http://www.con-dep.net");
            local.ToEachServer(server => server.ExecuteRemote.PowerShell("ipconfig"));
            local.ExecuteWebRequest("GET", "http://blog.torresdal.net");
        }
    }

    public class SequenceTestInfrastructure : InfrastructureArtifact
    {
        public override void Configure(IOfferInfrastructure require)
        {
            require.IIS();
        }
    }
}