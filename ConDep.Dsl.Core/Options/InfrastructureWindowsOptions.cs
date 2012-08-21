namespace ConDep.Dsl.Core
{
    public class InfrastructureWindowsOptions
    {
        private readonly WebDeployServerDefinition _webDeployServerDefinition;

        public InfrastructureWindowsOptions(WebDeployServerDefinition webDeployServerDefinition)
        {
            _webDeployServerDefinition = webDeployServerDefinition;
        }
    }
}