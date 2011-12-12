namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Configuration : IWebDeployModel
	{
		private bool _autoDeployAgent = true;

		public bool AutoDeployAgent
		{
			get { return _autoDeployAgent; }
			set { _autoDeployAgent = value; }
		}

		public bool IsValid(Notification notification)
		{
			return true;
		}
	}
}