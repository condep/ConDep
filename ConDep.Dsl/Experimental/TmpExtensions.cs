using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.Experimental.Core.Impl;
using ConDep.Dsl.WebDeployProviders.Deployment.CopyDir;
using ConDep.Dsl.WebDeployProviders.RunCmd;

namespace ConDep.Dsl.Experimental.Application
{
    public static class TmpExtensions
    {
        public static IOfferRemoteDeployment Directory2(this IOfferRemoteDeployment deployment, string sourceDir, string destDir)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            ExecutionSequenceManager.GetSequenceFor(deployment).Add(new RemoteOperation(copyDirProvider));
            return deployment;
        }

        public static IOfferRemoteExecution Directory3(this IOfferRemoteExecution execution, string cmd, bool continueOnError)
        {
            var runCmdProvider = new RunCmdProvider(cmd, continueOnError);
            ExecutionSequenceManager.GetSequenceFor(execution).Add(new RemoteOperation(runCmdProvider));
            return execution;
        }

        public static IOfferApplicationOps ExecuteWebRequest2(this IOfferApplicationOps appOps, string method, string url)
        {
            var operation = new Operations.WebRequest.WebRequestOperation(url, method);
            ExecutionSequenceManager.GetSequenceFor(appOps).Add(operation);
            return appOps;
        }
    }
}