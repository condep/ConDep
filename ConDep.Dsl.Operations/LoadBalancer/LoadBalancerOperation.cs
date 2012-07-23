using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Core
{
    public class LoadBalancerOperation : IOperateConDep
    {
        private readonly string _loadBalancerServerName;
        private readonly string _loadBalancerProvider;
        private readonly DeploymentServer _deploymentServer;
        private readonly DeploymentServer _previousDeploymentServer;

        public LoadBalancerOperation(string loadBalancerServerName, string loadBalancerProvider, DeploymentServer deploymentServer, DeploymentServer previousDeploymentServer)
        {
            _loadBalancerServerName = loadBalancerServerName;
            _loadBalancerProvider = loadBalancerProvider;
            _deploymentServer = deploymentServer;
            _previousDeploymentServer = previousDeploymentServer;
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
            return true;
        }

        public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            return null;
            //return _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);
            //foreach (var operation in _operations)
            //{
            //    operation.Execute(output, outputError, webDeploymentStatus);
            //}
            //return webDeploymentStatus;
        }
    }
}