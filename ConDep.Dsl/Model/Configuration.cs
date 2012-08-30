namespace ConDep.Dsl
{
	public class Configuration : IValidate
	{
        //todo: this must be false to not require WebDeploy on remote servers
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