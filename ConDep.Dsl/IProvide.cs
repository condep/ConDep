using System;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitInterval { get; set; }
        void AddCondition(IProvideConditions condition);
        IReportStatus Sync(WebDeployOptions webDeployOptions, IReportStatus status);
    }
}