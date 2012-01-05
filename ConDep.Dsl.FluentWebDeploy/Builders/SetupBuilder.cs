using System.Collections.Generic;
using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy.Builders
{
	public class SetupBuilder
	{
		private readonly SetupOperation _setupOperation;

		public SetupBuilder(SetupOperation setupOperation)
		{
			_setupOperation = setupOperation;
		}

		protected internal void AddOperation(IOperateConDep operation)
		{
			_setupOperation.AddOperation(operation);
		}
	}
}