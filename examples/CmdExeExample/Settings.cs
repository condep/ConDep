using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl;

namespace TestWebDeployApp
{
    public interface ISettings
    {
        string AppName { get; }
        string WebSiteName { get; }
        string BaseWebPath { get; }
        string BaseSvcPath { get; }
        string DestSvcPath { get; }
        string PreCompiledWebPath { get; }
        string SvcName { get; }

        IList<Server> Servers { get; }
        string Environment { get; }
        string WebFarm { get; }
        string ArrServer { get; }
        string ServiceUser { get; }
        string ServicePassword { get; }
        string[] WarmupUrls { get; }
    }

    public abstract class CrossEnvironmentSettings
    {        
        public string AppName { get { return "MyApp"; } }
        public string WebSiteName { get { return "Default Web Site"; } }
        public string BaseWebPath { get { return @"..\_PublishedWebsites\MyApp\"; } }
        public string BaseSvcPath { get { return @"..\_PublishedServices\MyApp.Endpoint\"; } }
        public string DestSvcPath { get { return @"e:\WindowsServices\MyApp.Endpoint\"; } }
        public string PreCompiledWebPath { get { return @"..\_PreCompiledWebsites\MyApp\"; } }
        public string SvcName { get { return "MyApp.Endpoint"; } }
    }

	public class DevSettings : CrossEnvironmentSettings, ISettings
	{
		public IList<Server> Servers
		{
            get 
            { 
                return new List<Server>
                           {
                               new Server { Ip = "10.0.1.21", Name = "devweb01", UserName = @"devDomain\Admin", Password = "67890" }, 
                               new Server { Ip = "10.0.1.31", Name = "devweb02", UserName = @"devDomain\Admin", Password = "67890" }
                           };
            }
		}

	    public string Environment { get { return "dev"; } }
	    public string WebFarm { get { return "Default"; } }
        public string ArrServer { get { return "devarr01"; } }
        public string ServiceUser { get { return @"devDomain\MyAppService"; } }
        public string ServicePassword { get { return "123456"; } }
        public string[] WarmupUrls { get { return Servers.Select(s => s.Ip + "/" + AppName).ToArray(); } }
    }

    public class TestSettings : CrossEnvironmentSettings, ISettings
    {
        public IList<Server> Servers
        {
            get
            {
                return new List<Server>
                           {
                               new Server { Ip = "10.0.2.21", Name = "testweb01" }, 
                               new Server { Ip = "10.0.2.31", Name = "testweb02" }
                           };
            }
        }

        public string Environment { get { return "test"; } }
        public string WebFarm { get { return "Default"; } }
        public string ArrServer { get { return "testarr01"; } }
        public string ServiceUser { get { return @"testDomain\MyAppService"; } }
        public string ServicePassword { get { return "123456"; } }
        public string[] WarmupUrls { get { return Servers.Select(s => s.Ip + "/" + AppName).ToArray(); } }
    }


    public class ProdSettings : CrossEnvironmentSettings, ISettings
    {
        public IList<Server> Servers
        {
            get
            {
                return new List<Server>
                           {
                               new Server { Ip = "10.0.3.21", Name = "prodweb01" }, 
                               new Server { Ip = "10.0.3.31", Name = "prodweb02" }
                           };
            }
        }

        public string Environment { get { return "prod"; } }
        public string WebFarm { get { return "Default"; } }
        public string ArrServer { get { return "prodarr01"; } }
        public string ServiceUser { get { return @"prodDomain\MyAppService"; } }
        public string ServicePassword { get { return "123456"; } }
        public string[] WarmupUrls { get { return Servers.Select(s => s.Ip + "/" + AppName).ToArray(); } }
    }
}