using System;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class MissingProviderException : Exception
	{
		public MissingProviderException(string message) : base(message) {}
	}
}