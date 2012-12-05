using Microsoft.Web.Deployment;

namespace ConDep.Dsl.SemanticModel.WebDeploy
{
	public class WebDeploySource : IValidate
	{
		private readonly Credentials _credentials = new Credentials();
        private bool _localHost = true;
	    private string _computerName;

	    public string ComputerName
	    {
	        get { return _computerName; }
	        set
	        {
	            _localHost = false;
	            _computerName = value;
	        }
	    }

        public bool LocalHost
	    {
	        get { return _localHost; }
	        set { _localHost = value; }
	    }

	    public bool HasCredentials
		{
			get { return !string.IsNullOrWhiteSpace(Credentials.UserName); }
		}

		public Credentials Credentials
		{
			get {
				return _credentials;
			}
		}

	    public string PackagePath { get; set; }

	    public string EncryptionPassword { get; set; }

	    public DeploymentBaseOptions GetSourceBaseOptions()
		{
			var sourceBaseOptions = new DeploymentBaseOptions();
			if (!LocalHost)
			{
				sourceBaseOptions.ComputerName = ComputerName;
			}

			if (HasCredentials)
			{
				sourceBaseOptions.UserName = Credentials.UserName;
				sourceBaseOptions.Password = Credentials.Password;
			}

            if(HasEncryptionPassword)
            {
                sourceBaseOptions.EncryptPassword = EncryptionPassword;
            }
			return sourceBaseOptions;
		}

	    protected bool HasEncryptionPassword
	    {
            get { return !string.IsNullOrWhiteSpace(EncryptionPassword); }
	    }

	    public bool IsValid(Notification notification)
		{
			_credentials.IsValid(notification);

			if (!LocalHost && string.IsNullOrWhiteSpace(ComputerName) && string.IsNullOrWhiteSpace(PackagePath))
			{
				notification.AddError(new SemanticValidationError("Neither localhost or computer name is defined for source.", ValidationErrorType.NoSource));
				return true;
			}
			return false;
		}
	}
}