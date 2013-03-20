using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    //null: Needs to be refactored and tests should get better names
    [TestFixture]
    public class LoadBalancerTests
    {
        private ConDepSettings _settingsStopAfterMarkedServer;
        private ConDepSettings _settingsContinueAfterMarkedServer;
        private ConDepSettings _settingsDefault;

        [SetUp]
        public void Setup()
        {
            _settingsStopAfterMarkedServer = new ConDepSettings
            {
                Options =
                    {
                        WebDeployExist = true,
                        StopAfterMarkedServer = true,
                        SuspendMode = LoadBalancerSuspendMethod.Graceful
                    }
            };

            _settingsContinueAfterMarkedServer = new ConDepSettings
            {
                Options =
                {
                    WebDeployExist = true,
                    ContinueAfterMarkedServer = true,
                    SuspendMode = LoadBalancerSuspendMethod.Graceful
                }
            };

            _settingsDefault = new ConDepSettings
            {
                Options =
                {
                    WebDeployExist = true,
                    SuspendMode = LoadBalancerSuspendMethod.Graceful
                }
            };
        }

        [Test]
        public void TestThatStickyLoadBlancingWithOneServerAndManuelTestWorks()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };

            config.Servers = new[] { server1 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.Sticky };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsStopAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));
        }

        [Test]
        public void TestThatStickyLoadBlancingWithOneServerAndContinueWorks()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };

            config.Servers = new[] { server1 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());

            var loadBalancer = new MockLoadBalancer { Mode = LbMode.Sticky };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsContinueAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("online"));
        }

        [Test]
        public void TestThatRoundRobinLoadBlancingWithOneServerAndManuelTestWorks()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };

            config.Servers = new[] { server1 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsStopAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));
        }

        [Test]
        public void TestThatRoundRobinLoadBlancingWithOneServerAndContinueWorks()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };

            config.Servers = new[] { server1 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsContinueAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("online"));
        }


        [Test]
        public void TestThatRoundRobinLoadBlancingWithOneServerWorks()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };

            config.Servers = new[] { server1 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsDefault);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(2));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item2, Is.EqualTo("online"));
        }

        [Test]
        public void TestThatStickyLoadBlancingWithOneServerWorks()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };

            config.Servers = new[] { server1 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.Sticky };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsDefault);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(config.Servers.Count * 2));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item2, Is.EqualTo("online"));
        }

        [Test]
        public void TestThatRoundRobinLoadBalancingGoesOnlineOfflineInCorrectOrder()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03" };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsDefault);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(config.Servers.Count * 2));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item1, Is.EqualTo("jat-web02"));
            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[2].Item1, Is.EqualTo("jat-web03"));
            Assert.That(loadBalancer.OnlineOfflineSequence[2].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[3].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[3].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[4].Item1, Is.EqualTo("jat-web02"));
            Assert.That(loadBalancer.OnlineOfflineSequence[4].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[5].Item1, Is.EqualTo("jat-web03"));
            Assert.That(loadBalancer.OnlineOfflineSequence[5].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[6].Item1, Is.EqualTo("jat-web04"));
            Assert.That(loadBalancer.OnlineOfflineSequence[6].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[7].Item1, Is.EqualTo("jat-web05"));
            Assert.That(loadBalancer.OnlineOfflineSequence[7].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[8].Item1, Is.EqualTo("jat-web04"));
            Assert.That(loadBalancer.OnlineOfflineSequence[8].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[9].Item1, Is.EqualTo("jat-web05"));
            Assert.That(loadBalancer.OnlineOfflineSequence[9].Item2, Is.EqualTo("online"));
        }

        [Test]
        public void TestThatStickyLoadBalancingGoesOnlineOfflineInCorrectOrder()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03" };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.Sticky };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsDefault);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(config.Servers.Count * 2));

            var serverNumber = 1;
            for (int i = 0; i < loadBalancer.OnlineOfflineSequence.Count; i += 2)
            {
                Assert.That(loadBalancer.OnlineOfflineSequence[i].Item1, Is.EqualTo("jat-web0" + serverNumber));
                Assert.That(loadBalancer.OnlineOfflineSequence[i].Item2, Is.EqualTo("offline"));

                Assert.That(loadBalancer.OnlineOfflineSequence[i + 1].Item1, Is.EqualTo("jat-web0" + serverNumber));
                Assert.That(loadBalancer.OnlineOfflineSequence[i + 1].Item2, Is.EqualTo("online"));
                serverNumber++;
            }
        }

        [Test]
        public void TestThatRoundRobinWithManualTestStopsAfterFirstServer()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03" };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsStopAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));
        }

        [Test]
        public void TestThatRoundRobinWithContinueAfterManuelTestOnSpecificServerExecuteCorrectServers()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03", StopServer = true };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsStopAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web03"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));
        }

        [Test]
        public void TestThatStickyWithContinueAfterManualTestExecutesOnCorrectServers()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03" };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.Sticky };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsContinueAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(((config.Servers.Count - 1) * 2) + 1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("online"));

            var serverNumber = 2;
            for (int i = 1; i < loadBalancer.OnlineOfflineSequence.Count; i += 2)
            {
                Assert.That(loadBalancer.OnlineOfflineSequence[i].Item1, Is.EqualTo("jat-web0" + serverNumber));
                Assert.That(loadBalancer.OnlineOfflineSequence[i].Item2, Is.EqualTo("offline"));

                Assert.That(loadBalancer.OnlineOfflineSequence[i + 1].Item1, Is.EqualTo("jat-web0" + serverNumber));
                Assert.That(loadBalancer.OnlineOfflineSequence[i + 1].Item2, Is.EqualTo("online"));
                serverNumber++;
            }
        }

        [Test]
        public void TestThatStickyWithContinueAfterManualTestOnSpecificServerExecutesOnCorrectServers()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03", StopServer = true };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.Sticky };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsContinueAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(((config.Servers.Count - 1) * 2) + 1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web03"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[2].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[2].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[3].Item1, Is.EqualTo("jat-web02"));
            Assert.That(loadBalancer.OnlineOfflineSequence[3].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[4].Item1, Is.EqualTo("jat-web02"));
            Assert.That(loadBalancer.OnlineOfflineSequence[4].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[5].Item1, Is.EqualTo("jat-web04"));
            Assert.That(loadBalancer.OnlineOfflineSequence[5].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[6].Item1, Is.EqualTo("jat-web04"));
            Assert.That(loadBalancer.OnlineOfflineSequence[6].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[7].Item1, Is.EqualTo("jat-web05"));
            Assert.That(loadBalancer.OnlineOfflineSequence[7].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[8].Item1, Is.EqualTo("jat-web05"));
            Assert.That(loadBalancer.OnlineOfflineSequence[8].Item2, Is.EqualTo("online"));
        }

        [Test]
        public void TestThatRoundRobinWithContinueAfterManualTestOnSpecificServerExecutesOnCorrectServers()
        {
            var config = new ConDepEnvConfig { EnvironmentName = "bogusEnv" };
            var server1 = new ServerConfig { Name = "jat-web01" };
            var server2 = new ServerConfig { Name = "jat-web02" };
            var server3 = new ServerConfig { Name = "jat-web03", StopServer = true };
            var server4 = new ServerConfig { Name = "jat-web04" };
            var server5 = new ServerConfig { Name = "jat-web05" };

            config.Servers = new[] { server1, server2, server3, server4, server5 };

            var infrastructureSequence = new InfrastructureSequence();
            var preOpsSequence = new PreOpsSequence(new WebDeployHandlerMock());
            var loadBalancer = new MockLoadBalancer { Mode = LbMode.RoundRobin };

            var remoteSequence = new RemoteSequence(infrastructureSequence, preOpsSequence, config.Servers, loadBalancer);

            var status = new StatusReporter();
            remoteSequence.Execute(status, _settingsContinueAfterMarkedServer);

            Assert.That(loadBalancer.OnlineOfflineSequence.Count, Is.EqualTo(((config.Servers.Count - 1) * 2) + 1));

            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[0].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item1, Is.EqualTo("jat-web02"));
            Assert.That(loadBalancer.OnlineOfflineSequence[1].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[2].Item1, Is.EqualTo("jat-web03"));
            Assert.That(loadBalancer.OnlineOfflineSequence[2].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[3].Item1, Is.EqualTo("jat-web01"));
            Assert.That(loadBalancer.OnlineOfflineSequence[3].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[4].Item1, Is.EqualTo("jat-web02"));
            Assert.That(loadBalancer.OnlineOfflineSequence[4].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[5].Item1, Is.EqualTo("jat-web04"));
            Assert.That(loadBalancer.OnlineOfflineSequence[5].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[6].Item1, Is.EqualTo("jat-web05"));
            Assert.That(loadBalancer.OnlineOfflineSequence[6].Item2, Is.EqualTo("offline"));

            Assert.That(loadBalancer.OnlineOfflineSequence[7].Item1, Is.EqualTo("jat-web04"));
            Assert.That(loadBalancer.OnlineOfflineSequence[7].Item2, Is.EqualTo("online"));

            Assert.That(loadBalancer.OnlineOfflineSequence[8].Item1, Is.EqualTo("jat-web05"));
            Assert.That(loadBalancer.OnlineOfflineSequence[8].Item2, Is.EqualTo("online"));
        }
         
    }
}