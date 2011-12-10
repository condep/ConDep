using System;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class MissingDestinationExcepton : Exception
	{
		public MissingDestinationExcepton(string message) : base(message) { }
	}
}