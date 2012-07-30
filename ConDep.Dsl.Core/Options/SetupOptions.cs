namespace ConDep.Dsl.Core
{
	public class SetupOptions
	{
		private readonly SetupOperation _setupOperation;

	    public SetupOptions(SetupOperation setupOperation)
	    {
	        _setupOperation = setupOperation;
	    }

	    public void AddOperation(ConDepOperation operation)
        {
            _setupOperation.AddOperation(operation);
        }
	}
}