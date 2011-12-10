using System;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class MissingSourceException : Exception
	{
		public MissingSourceException(string message) : base(message)
		{
			
		}
	}
}