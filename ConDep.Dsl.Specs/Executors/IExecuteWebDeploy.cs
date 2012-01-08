using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl.Specs.Executors
{
    public interface IExecuteWebDeploy
    {
        WebDeploymentStatus Execute();
        WebDeploymentStatus ExecuteFromPackage();
    }
}