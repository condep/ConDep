namespace ConDep.Dsl
{
    public class CustomWebAppOptions
    {
        private readonly CustomWebAppProvider _webAppProvider;

        public CustomWebAppOptions(CustomWebAppProvider webAppProvider)
        {
            _webAppProvider = webAppProvider;
        }

        public string PhysicalPath
        {
            get { return _webAppProvider.PhysicalPath; } 
            set { _webAppProvider.PhysicalPath = value; }
        }

        public string ApplicationPool
        {
            get { return _webAppProvider.ApplicationPool; } 
            set { _webAppProvider.ApplicationPool = value; }
        }
    }
}