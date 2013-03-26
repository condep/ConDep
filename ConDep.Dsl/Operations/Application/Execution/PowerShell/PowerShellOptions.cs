namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class PowerShellOptions : IOfferPowerShellOptions
    {
        private readonly PowerShellOperation _powerShellOperation;

        public PowerShellOptions(PowerShellOperation powerShellOperation)
        {
            _powerShellOperation = powerShellOperation;
        }

        public IOfferPowerShellOptions RequireRemoteLib()
        {
            _powerShellOperation.RequireRemoteLib = true;
            return this;
        }

        public IOfferPowerShellOptions ContinueOnError(bool value)
        {
            _powerShellOperation.ContinueOnError = value;
            return this;
        }

        public IOfferPowerShellOptions WaitIntervalInSeconds(int seconds)
        {
            _powerShellOperation.WaitIntervalInSeconds = seconds;
            return this;
        }

        public IOfferPowerShellOptions RetryAttempts(int attempts)
        {
            _powerShellOperation.RetryAttempts = attempts;
            return this;
        }
    }
}