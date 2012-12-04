using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.WebDeploy
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitInterval { get; set; }
        void AddCondition(IProvideConditions condition);
        IReportStatus Sync(WebDeployOptions webDeployOptions, IReportStatus status);
    }
}