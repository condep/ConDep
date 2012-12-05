namespace ConDep.Dsl.SemanticModel.WebDeploy
{
    public interface IProvide
    {
        bool IsValid(Notification notification);
		int WaitInterval { get; set; }
        IReportStatus Sync(WebDeployOptions webDeployOptions, IReportStatus status);
    }
}