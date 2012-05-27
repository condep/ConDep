namespace ConDep.Dsl
{
    public class PowerShellOptions
    {
        private readonly PowerShellProvider _powerShellProvider;

        public PowerShellOptions(PowerShellProvider powerShellProvider)
        {
            _powerShellProvider = powerShellProvider;
        }

        public PowerShellOptions ContinueOnError()
        {
            _powerShellProvider.ContinueOnError = true;
            return this;
        }

        public PowerShellOptions WaitIntervalInSeconds(int seconds)
        {
            _powerShellProvider.WaitInterval = seconds;
            return this;
        }
    }
}