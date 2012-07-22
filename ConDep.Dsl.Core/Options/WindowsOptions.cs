namespace ConDep.Dsl.Core
{
    public class WindowsOptions
    {
        private readonly WebDeployDefinition _webDeployDefinition;

        public WindowsOptions(WebDeployDefinition webDeployDefinition)
        {
            _webDeployDefinition = webDeployDefinition;
        }

        public void InstallIIS()
        {
            _webDeployDefinition.WebDeploySource.LocalHost = true;
            //iisDefinition(new ProviderOptions(_webDeployDefinition.Providers));
        }
    }
}