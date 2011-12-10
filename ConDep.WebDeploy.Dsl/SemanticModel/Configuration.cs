namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Configuration : IWebDeployModel
	{
		public bool AutoDeployAgent { get; set; }
		public bool IsValid(Notification notification)
		{
			return true;
		}
	}
}