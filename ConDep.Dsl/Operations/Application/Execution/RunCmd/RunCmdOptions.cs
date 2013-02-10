namespace ConDep.Dsl.Operations.Application.Execution.RunCmd
{
    public class RunCmdOptions : IOfferRunCmdOptions
    {
		private readonly RunCmdProvider _provider;

		public RunCmdOptions(RunCmdProvider provider)
		{
			_provider = provider;
		}

        public IOfferRunCmdOptions WaitIntervalInSeconds(int waitInterval)
		{
			_provider.WaitIntervalInSeconds = waitInterval;
			return this;
		}

        public IOfferRunCmdOptions RetryAttempts(int retryAttempts)
	    {
            _provider.RetryAttempts = retryAttempts;
            return this;
        }

        public IOfferRunCmdOptions ContinueOnError(bool continueOnError)
        {
            _provider.ContinueOnError = continueOnError;
            return this;
        }
	}
}