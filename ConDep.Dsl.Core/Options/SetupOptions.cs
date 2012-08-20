namespace ConDep.Dsl.Core
{
	public class SetupOptions
	{
		private readonly ConDepSetup _conDepSetup;

	    public SetupOptions(ConDepSetup conDepSetup)
	    {
	        _conDepSetup = conDepSetup;
	    }

	    public void AddOperation(ConDepOperation operation)
        {
            _conDepSetup.AddOperation(operation);
        }
	}
}