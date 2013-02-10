namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class PowerShellOptions : IOfferPowerShellOptions
    {
        private readonly PowerShellProvider _powerShellProvider;

        public PowerShellOptions(PowerShellProvider powerShellProvider)
        {
            _powerShellProvider = powerShellProvider;
        }

        public IOfferPowerShellOptions RequireRemoteLib()
        {
            _powerShellProvider.RequireRemoteLib = true;
            return this;
        }

        public IOfferPowerShellOptions ContinueOnError(bool value)
        {
            _powerShellProvider.ContinueOnError = value;
            return this;
        }

        public IOfferPowerShellOptions WaitIntervalInSeconds(int seconds)
        {
            _powerShellProvider.WaitIntervalInSeconds = seconds;
            return this;
        }

        public IOfferPowerShellOptions RetryAttempts(int attempts)
        {
            _powerShellProvider.RetryAttempts = attempts;
            return this;
        }
    }
}