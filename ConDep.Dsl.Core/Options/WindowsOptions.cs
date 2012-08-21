namespace ConDep.Dsl.Core
{
    public class WindowsOptions
    {
        private readonly WebDeployServerDefinition _webDeployServerDefinition;

        public WindowsOptions(WebDeployServerDefinition webDeployServerDefinition)
        {
            _webDeployServerDefinition = webDeployServerDefinition;
        }

        public void InstallIIS()
        {
            _webDeployServerDefinition.WebDeploySource.LocalHost = true;
            //iisDefinition(new ProviderOptions(_webDeployDefinition.Providers));
        }
    }
}