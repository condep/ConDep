using ConDep.Dsl.Operations;

namespace ConDep.Dsl.Builders
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