using System;
using ConDep.Dsl.Core;

namespace ConDep.Dsl.Builders
{
    public class LoadBalancerOperation : IOperateConDep
    {
        private readonly WebDeployDefinition _webDeployDefinition;
        //private readonly List<IOperateConDep> _operations = new List<IOperateConDep>();

        public LoadBalancerOperation(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        //public void AddOperation(IOperateConDep operation)
        //{
        //    _operations.Add(operation);
        //}

        public bool IsValid(Notification notification)
        {
            //return _operations.All(operation => operation.IsValid(notification));
            return _webDeployDefinition.IsValid(notification);
        }

        public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            return _webDeployDefinition.Sync(output, outputError, webDeploymentStatus);
            //foreach (var operation in _operations)
            //{
            //    operation.Execute(output, outputError, webDeploymentStatus);
            //}
            //return webDeploymentStatus;
        }
    }
}