namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class PowerShellOptions
    {
        private readonly PowerShellProvider _powerShellProvider;

        public PowerShellOptions(PowerShellProvider powerShellProvider)
        {
            _powerShellProvider = powerShellProvider;
        }

        public PowerShellOptions RequireRemoteLib()
        {
            _powerShellProvider.RequireRemoteLib = true;
            return this;
        }

        public PowerShellOptions ContinueOnError(bool value)
        {
            _powerShellProvider.ContinueOnError = value;
            return this;
        }

        public PowerShellOptions WaitIntervalInSeconds(int seconds)
        {
            _powerShellProvider.WaitIntervalInSeconds = seconds;
            return this;
        }

        public PowerShellOptions RetryAttempts(int attempts)
        {
            _powerShellProvider.RetryAttempts = attempts;
            return this;
        }
    }
}