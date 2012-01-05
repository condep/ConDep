namespace ConDep.Dsl.Operations.WebDeploy.Model
{
	public class Configuration : IValidate
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