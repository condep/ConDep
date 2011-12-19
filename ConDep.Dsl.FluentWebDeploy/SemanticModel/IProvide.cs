using ConDep.Dsl.FluentWebDeploy.Deployment;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitInterval { get; set; }
        DeploymentStatus Execute(WebDeployOptions webDeployOptions, DeploymentStatus deploymentStatus);
    }
}