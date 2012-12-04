namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebApp
{
    public class WebAppInfrastructureOptions
    {
        private readonly WebAppInfrastructureProvider _webAppInfrastructureProvider;

        public WebAppInfrastructureOptions(WebAppInfrastructureProvider webAppInfrastructureProvider)
        {
            _webAppInfrastructureProvider = webAppInfrastructureProvider;
        }

        public string PhysicalPath
        {
            get { return _webAppInfrastructureProvider.PhysicalPath; } 
            set { _webAppInfrastructureProvider.PhysicalPath = value; }
        }

        public string ApplicationPool
        {
            get { return _webAppInfrastructureProvider.ApplicationPool; } 
            set { _webAppInfrastructureProvider.ApplicationPool = value; }
        }
    }
}