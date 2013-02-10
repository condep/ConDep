using System.IO;
using System.Text;
using ConDep.Dsl.Config;
using NUnit.Framework;
using System.Linq;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class JsonParsingTests
    {
        private string _tiersJson =
            @"{
	""Tiers"" :
	[
        {
            ""Name"" : ""Web"",
			""Servers"" :
			[
				{
					""Name"" : ""jat-web01""
				},
				{
					""Name"" : ""jat-web02""
				}
			],
            ""LoadBalancer"": 
            {
                ""Name"": ""jat-nlb01"",
                ""Provider"": ""ConDep.Dsl.LoadBalancer.Ace.dll"",
                ""UserName"": ""torresdal\\nlbUser"",
                ""Password"": ""verySecureP@ssw0rd"",
                ""Mode"": ""Sticky""
            }
        },
        {
            ""Name"" : ""Application"",
			""Servers"" :
			[
				{
					""Name"" : ""jat-app01""
				},
				{
					""Name"" : ""jat-app02""
				}
			]
        },
        {
            ""Name"" : ""Database"",
			""Servers"" :
			[
				{
					""Name"" : ""jat-db01""
				},
				{
					""Name"" : ""jat-db02""
				}
			]
        }
	],
    ""DeploymentUser"": 
    {
        ""UserName"": ""torresdal\\condepuser"",
        ""Password"": ""verySecureP@ssw0rd""
    },
    ""CustomProviderConfig"":
    [
        {
            ""ProviderName"" : ""NServiceBusProvider"",
            ""ProviderConfig"": 
            {
                ""ServiceUserName"": ""torresdal\\nservicebususer"",
                ""ServicePassword"": ""verySecureP@ssw0rd""
            }
        },
        {
            ""ProviderName"" : ""SomeOtherProvider"",
            ""ProviderConfig"":
            {
                ""SomeOtherSetting1"": ""asdfasdf"",
                ""SomeOtherSetting2"": ""34tsdfg""
            }
        }
    ]
}";

        private string _json =
            @"{
    ""LoadBalancer"": 
    {
        ""Name"": ""jat-nlb01"",
        ""Provider"": ""ConDep.Dsl.LoadBalancer.Ace.dll"",
        ""UserName"": ""torresdal\\nlbUser"",
        ""Password"": ""verySecureP@ssw0rd"",
        ""Mode"": ""Sticky""
    },
	""Servers"":
    [
        {
            ""Name"" : ""jat-web01"",
            ""LoadBalancerFarm"": ""farm1"",
            ""StopServer"": true,
		    ""WebSites"" : 
		    [
			    { 
                    ""Name"" : ""WebSite1"", 
                    ""Bindings"": 
                    [
                        { ""BindingType"": ""http"", ""Port"" : ""80"", ""Ip"" : ""10.0.0.111"", ""HostHeader"" : """" },
                        { ""BindingType"": ""https"", ""Port"" : ""443"", ""Ip"" : ""10.0.0.111"", ""HostHeader"" : """" }
                    ]
                },
			    { 
                    ""Name"" : ""WebSite2"", 
                    ""Bindings"": 
                    [
                        { ""BindingType"": ""http"", ""Port"" : ""80"", ""Ip"" : ""10.0.0.112"", ""HostHeader"" : """" },
                        { ""BindingType"": ""https"", ""Port"" : ""443"", ""Ip"" : ""10.0.0.112"", ""HostHeader"" : """" }
                    ]
                },
			    { 
                    ""Name"" : ""WebSite3"", 
                    ""Bindings"": 
                    [
                        { ""BindingType"": ""http"", ""Port"" : ""80"", ""Ip"" : ""10.0.0.113"", ""HostHeader"" : """" },
                        { ""BindingType"": ""https"", ""Port"" : ""443"", ""Ip"" : ""10.0.0.113"", ""HostHeader"" : """" }
                    ]
                }
			]
        },
        {
            ""Name"" : ""jat-web02"",
            ""LoadBalancerFarm"": ""farm1"",
		    ""WebSites"" : 
		    [
			    { 
                    ""Name"" : ""WebSite1"", 
                    ""Bindings"": 
                    [
                        { ""BindingType"": ""http"", ""Port"" : ""80"", ""Ip"" : ""10.0.0.121"", ""HostHeader"" : """" },
                        { ""BindingType"": ""https"", ""Port"" : ""443"", ""Ip"" : ""10.0.0.121"", ""HostHeader"" : """" }
                    ]
                },
			    { 
                    ""Name"" : ""WebSite2"", 
                    ""Bindings"": 
                    [
                        { ""BindingType"": ""http"", ""Port"" : ""80"", ""Ip"" : ""10.0.0.122"", ""HostHeader"" : """" },
                        { ""BindingType"": ""https"", ""Port"" : ""443"", ""Ip"" : ""10.0.0.122"", ""HostHeader"" : """" }
                    ]
                },
			    { 
                    ""Name"" : ""WebSite3"", 
                    ""Bindings"": 
                    [
                        { ""BindingType"": ""http"", ""Port"" : ""80"", ""Ip"" : ""10.0.0.123"", ""HostHeader"" : """" },
                        { ""BindingType"": ""https"", ""Port"" : ""443"", ""Ip"" : ""10.0.0.123"", ""HostHeader"" : """" }
                    ]
                }
			]
        }
    ],
    ""DeploymentUser"": 
    {
        ""UserName"": ""torresdal\\condepuser"",
        ""Password"": ""verySecureP@ssw0rd""
    },
    ""CustomProviderConfig"":
    [
        {
            ""ProviderName"" : ""NServiceBusProvider"",
            ""ProviderConfig"": 
            {
                ""ServiceUserName"": ""torresdal\\nservicebususer"",
                ""ServicePassword"": ""verySecureP@ssw0rd""
            }
        },
        {
            ""ProviderName"" : ""SomeOtherProvider"",
            ""ProviderConfig"":
            {
                ""SomeOtherSetting1"": ""asdfasdf"",
                ""SomeOtherSetting2"": ""34tsdfg""
            }
        }
    ]
}";

        private ConDepConfig _config;
        private ConDepConfig _tiersConfig;

        [SetUp]
        public void Setup()
        {
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_json));
            var tiersMemStream = new MemoryStream(Encoding.UTF8.GetBytes(_tiersJson));

            var parser = new EnvConfigParser();
            _config = parser.GetEnvConfig(memStream);
            _tiersConfig = parser.GetEnvConfig(tiersMemStream);
        }

        [Test]
        public void TestThatLoadBalancerExist()
        {
            Assert.That(_config.LoadBalancer, Is.Not.Null);
        }

        [Test]
        public void TestThatLoadBalancerHasValuesInAllFields()
        {
            Assert.That(_config.LoadBalancer.Name, Is.Not.Null.Or.Empty);
            Assert.That(_config.LoadBalancer.Password, Is.Not.Null.Or.Empty);
            Assert.That(_config.LoadBalancer.Provider, Is.Not.Null.Or.Empty);
            Assert.That(_config.LoadBalancer.UserName, Is.Not.Null.Or.Empty);
            Assert.That(_config.LoadBalancer.Mode, Is.Not.Null.Or.Empty);
            Assert.That(_config.LoadBalancer.ModeAsEnum, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void TestThatDeploymentUserExist()
        {
            Assert.That(_config.DeploymentUser, Is.Not.Null);
        }

        [Test]
        public void TestThatDeploymentUserHasValuesInAllFields()
        {
            Assert.That(_config.DeploymentUser.UserName, Is.Not.Null.Or.Empty);
            Assert.That(_config.DeploymentUser.Password, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void TestThatCustomProviderConfigExist()
        {
            Assert.That(_config.CustomProviderConfig, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void TestThatCustomProviderHasExactlyTwoProviders()
        {
            Assert.That(_config.CustomProviderConfig.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestThatCustomProvidersHasValues()
        {
            foreach(var provider in _config.CustomProviderConfig)
            {
                Assert.That(provider.ProviderConfig, Is.Not.Null.Or.Empty);
                Assert.That(provider.ProviderName, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void TestThatCustomProvidersHasProviderConfigValues()
        {
            foreach (var provider in _config.CustomProviderConfig)
            {
                foreach(var config in provider.ProviderConfig)
                {
                    Assert.That(config.Key, Is.Not.Null.Or.Empty);
                    Assert.That(config.Value, Is.Not.Null.Or.Empty);
                }
            }
        }

        [Test]
        public void TestThatServersExist()
        {
            Assert.That(_config.Servers, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void TestThatServersContainsExactlyTwo()
        {
            Assert.That(_config.Servers.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestThatServersHaveNames()
        {
            foreach (var server in _config.Servers)
            {
                Assert.That(server.Name, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void TestThatOnlyOneServerIsTestServer()
        {
            var value = _config.Servers.Single(x => x.StopServer);
            Assert.That(value.StopServer);
        }

        [Test]
        public void TestThatServersHaveFarm()
        {
            foreach(var server in _config.Servers)
            {
                Assert.That(server.LoadBalancerFarm, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void TestThatServersHaveWebSites()
        {
            foreach (var server in _config.Servers)
            {
                Assert.That(server.WebSites, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void TestThatServersHaveWebSitesWithNames()
        {
            foreach (var server in _config.Servers)
            {
                foreach (var webSite in server.WebSites)
                {
                    Assert.That(webSite.Name, Is.Not.Null.Or.Empty);
                }
            }
        }

        [Test]
        public void TestThatServersHaveWebSitesWithBindings()
        {
            foreach (var server in _config.Servers)
            {
                foreach (var webSite in server.WebSites)
                {
                    Assert.That(webSite.Bindings, Is.Not.Null.Or.Empty);
                }
            }
        }

        [Test]
        public void TestThatServersHaveWebSitesWithExactlyTwoBindings()
        {
            foreach (var server in _config.Servers)
            {
                foreach (var webSite in server.WebSites)
                {
                    Assert.That(webSite.Bindings.Count, Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void TestThatServersHaveWebSitesWithBindingsWithValues()
        {
            foreach (var server in _config.Servers)
            {
                foreach (var webSite in server.WebSites)
                {
                    foreach (var binding in webSite.Bindings)
                    {
                        Assert.That(binding.BindingType, Is.Not.Null.Or.Empty);
                        Assert.That(binding.Ip, Is.Not.Null.Or.Empty);
                        Assert.That(binding.Port, Is.Not.Null.Or.Empty);
                        Assert.That(binding.HostHeader, Is.Not.Null);
                        Assert.That(binding.HostHeader, Is.Empty);
                    }
                }
            }
        }

        [Test]
        public void TestThatRootDeploymentUserIsInheritedForServersUnlessExplicitlyDefined()
        {
            foreach (var server in _config.Servers)
            {
                Assert.That(server.DeploymentUser, Is.SameAs(_config.DeploymentUser));
            }
        }
    }
}