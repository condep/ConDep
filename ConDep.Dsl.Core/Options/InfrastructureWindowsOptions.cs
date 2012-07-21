namespace ConDep.Dsl.Core
{
    public class InfrastructureWindowsOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public InfrastructureWindowsOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }
    }
}