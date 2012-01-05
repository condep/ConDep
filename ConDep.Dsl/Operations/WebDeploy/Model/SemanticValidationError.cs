namespace ConDep.Dsl.Operations.WebDeploy.Model
{
	public class SemanticValidationError
	{
		private readonly string _message;
		private readonly ValidationErrorType _errorType;

		public SemanticValidationError(string message, ValidationErrorType errorType)
		{
			_message = message;
			_errorType = errorType;
		}

		public string Message
		{
			get { return _message; }
		}

		public ValidationErrorType ErrorType
		{
			get { return _errorType; }
		}

	}
}