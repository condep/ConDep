using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Deployment.CopyDir;
using ConDep.Dsl.Operations.Application.Execution.PowerShell;
using ConDep.Dsl.Operations.Application.Local.WebRequest;
using ConDep.Dsl.Operations.Infrastructure.IIS.WebSite;

namespace ConDep.Dsl.Experimental
{
    public static class TmpExtensions
    {

        //Local operation
        public static IOfferLocalOperations WebRequestPut(this IOfferLocalOperations local, string url)
        {
            var op = new WebRequestOperation(url, "PUT");
            Configure.LocalOperations.AddOperation(op);
            return local;
        }

        //Remote Deployment Operation
        public static IOfferRemoteDeployment WebRequestPut(this IOfferRemoteDeployment local, string url)
        {
            var op = new CopyDirProvider(url, "PUT");
            Configure.DeploymentOperations.AddOperation(op);
            return local;
        }

        //Remote Execution Operation
        public static IOfferRemoteExecution WebRequestPut(this IOfferRemoteExecution local, string url)
        {
            var op = new PowerShellProvider("");
            Configure.ExecutionOperations.AddOperation(op);
            return local;
        }

        //Infrastructure Operation
        public static IOfferInfrastructure WebRequestPut(this IOfferInfrastructure local, string url)
        {
            var op = new IisWebSiteOperation("asdf", 5);
            Configure.InfrastructureOperations.AddOperation(op);
            return local;
        }
    }
}