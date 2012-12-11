using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Deployment.CopyDir;
using ConDep.Dsl.Operations.Application.Execution.RunCmd;
using ConDep.Dsl.Operations.Application.Local.WebRequest;
using ConDep.Dsl.SemanticModel;
using ConDep.Dsl.SemanticModel.Sequence;

namespace ConDep.Dsl.Experimental
{
    public static class TmpExtensions
    {
        public static IOfferRemoteDeployment Directory2(this IOfferRemoteDeployment deployment, string sourceDir, string destDir)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            ExecutionSequenceManager.GetSequenceFor(deployment).Add(new RemoteWebDeployOperation(copyDirProvider));
            return deployment;
        }

        public static IOfferRemoteExecution Directory3(this IOfferRemoteExecution execution, string cmd, bool continueOnError)
        {
            var runCmdProvider = new RunCmdProvider(cmd, continueOnError);
            ExecutionSequenceManager.GetSequenceFor(execution).Add(new RemoteWebDeployOperation(runCmdProvider));
            return execution;
        }

        public static IOfferLocalOperations ExecuteWebRequest2(this IOfferLocalOperations appOps, string method, string url)
        {
            var operation = new WebRequestOperation(url, method);
            ExecutionSequenceManager.GetSequenceFor(appOps).Add(operation);
            return appOps;
        }
    }
}