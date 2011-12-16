namespace ConDep.Dsl.FluentWebDeploy
{
    public class PowerShellBuilder
    {
        private readonly PowerShellProvider _powerShellProvider;

        public PowerShellBuilder(PowerShellProvider powerShellProvider)
        {
            _powerShellProvider = powerShellProvider;
        }
    }
}