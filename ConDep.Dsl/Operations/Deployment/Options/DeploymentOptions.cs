using ConDep.Dsl.Operations;

namespace ConDep.Dsl.Builders
{
	public class DeploymentOptions
	{
		private readonly SetupOperation _setupOperation;

	    public DeploymentOptions(SetupOperation setupOperation)
		{
			_setupOperation = setupOperation;
		}

	    public void AddOperation(IOperateConDep operation)
		{
			_setupOperation.AddOperation(operation);
		}
	}
}