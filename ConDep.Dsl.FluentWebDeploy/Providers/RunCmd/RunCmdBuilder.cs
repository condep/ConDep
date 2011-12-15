namespace ConDep.Dsl.FluentWebDeploy
{
	public class RunCmdBuilder
	{
		private readonly RunCmdProvider _provider;

		public RunCmdBuilder(RunCmdProvider provider)
		{
			_provider = provider;
		}

		public RunCmdBuilder WaitIntervalInSeconds(int waitInterval)
		{
			_provider.WaitInterval = waitInterval;
			return this;
		}
	}
}