using System;

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

	    public bool UseWhatIf { get; set; }

	    public bool IsValid(Notification notification)
		{
			return true;
		}
	}
}