using System.Collections.Generic;
using System.Linq;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class Notification
	{
		private List<SemanticValidationError> _validationErrors = new List<SemanticValidationError>();

		public bool HasErrors
		{
			get { return _validationErrors.Count > 0; }
		}

		public void AddError(SemanticValidationError error)
		{
			_validationErrors.Add(error);
		}

		public bool HasErrorOfType(ValidationErrorType errorType)
		{
			return _validationErrors.Any(e => e.ErrorType == errorType);
		}
	}
}