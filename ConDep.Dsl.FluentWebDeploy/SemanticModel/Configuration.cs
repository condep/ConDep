namespace ConDep.Dsl.FluentWebDeploy.SemanticModel
{
	public class Configuration : IWebDeployModel
	{
		private bool _doNotAutoDeployAgent = true;

		public bool DoNotAutoDeployAgent
		{
			get { return _doNotAutoDeployAgent; }
			set { _doNotAutoDeployAgent = value; }
		}

		public bool IsValid(Notification notification)
		{
			return true;
		}
	}
}