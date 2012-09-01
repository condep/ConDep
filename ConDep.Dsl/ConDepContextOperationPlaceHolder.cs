using System;
using System.Diagnostics;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public class ConDepContextOperationPlaceHolder : ConDepOperationBase
    {
        private string _contextName;

        public ConDepContextOperationPlaceHolder(string contextName)
        {
            ContextName = contextName;
        }

        public string ContextName
        {
            get { return _contextName; }
            set { _contextName = value; }
        }

        public override WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}