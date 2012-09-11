using System;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitInterval { get; set; }
        void AddCondition(IProvideConditions condition);
        WebDeploymentStatus Sync(WebDeployOptions webDeployOptions, WebDeploymentStatus deploymentStatus);
    }
}