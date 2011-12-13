using Microsoft.Web.Deployment;

namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
    }
}