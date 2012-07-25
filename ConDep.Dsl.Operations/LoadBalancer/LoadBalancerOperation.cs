using System;
using System.Linq;
using System.Reflection;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Core
{
    public class LoadBalancerOperation : IOperateConDep
    {
        private readonly string _loadBalancerServerName;
        private readonly string _loadBalancerProvider;
        private readonly DeploymentServer _deploymentServer;
        private readonly DeploymentServer _previousDeploymentServer;
        private IOperateConDep _loadBalancer;

        public LoadBalancerOperation(string loadBalancerServerName, string loadBalancerProvider, DeploymentServer deploymentServer, DeploymentServer previousDeploymentServer)
        {
            _loadBalancerServerName = loadBalancerServerName;
            _loadBalancerProvider = loadBalancerProvider;
            _deploymentServer = deploymentServer;
            _previousDeploymentServer = previousDeploymentServer;

            FindLoadBalancerProvider(loadBalancerProvider, loadBalancerServerName, deploymentServer, previousDeploymentServer);
        }

        private void FindLoadBalancerProvider(string loadBalancerProvider, string loadBalancerServerName, DeploymentServer deploymentServer, DeploymentServer previousDeploymentServer)
        {
            var assembly = Assembly.Load(loadBalancerProvider);
            var type = assembly.GetTypes().Where(t => typeof(ILoadBalance).IsAssignableFrom(t)).FirstOrDefault();
            _loadBalancer = Activator.CreateInstance(type, loadBalancerServerName, deploymentServer, previousDeploymentServer) as IOperateConDep;
        }

        //private readonly WebDeployDefinition _webDeployDefinition;
        //private readonly List<IOperateConDep> _operations = new List<IOperateConDep>();

        //public LoadBalancerOperation(WebDeployDefinition webDeployDefinition)
        //{
        //    _webDeployDefinition = webDeployDefinition;
        //}

        //public void AddOperation(IOperateConDep operation)
        //{
        //    _operations.Add(operation);
        //}

        public bool IsValid(Notification notification)
        {

            //return _operations.All(operation => operation.IsValid(notification));
            //return _webDeployDefinition.IsValid(notification);
            return _loadBalancer != null;
        }

        public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            return _loadBalancer.Execute(output, outputError, webDeploymentStatus);
            //return _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);
            //foreach (var operation in _operations)
            //{
            //    operation.Execute(output, outputError, webDeploymentStatus);
            //}
            //return webDeploymentStatus;
        }
    }
}