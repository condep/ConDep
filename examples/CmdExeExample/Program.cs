using ConDep.Dsl;

namespace TestWebDeployApp
{
    //ToDo: Use ILMerge to get everything into one .exe
    public class Program : ConDepConsoleApp<Program, DeploymentSettings>
    {
        static void Main(string[] args)
        {
            Initialize(args);
        }

        protected override void Execute()
        {
            Setup(setup =>
                      {
                          setup.TransformWebConfig(@"C:\Temp\MyApp", "web.config", "web.test.config");
                          setup.PreCompile("MyApp", @"C:\Temp\MyApp", @"C:\temp\Test\MyApp");
								  //setup.ApplicationRequestRouting("server",
								  //                                arr => arr.TakeFarmOfflineForServer("10.0.0.21", "Farm1"));
								  setup.ApplicationRequestRouting("ffdevnlb01").LoadBalancer.Farm("Sikker").TakeServerOffline("10.70.148.72"); //, a => a.TakeFarmOfflineForServer(Settings.ArrServers.DevServers[Settings.WebServer], Settings.ArrFarmName));

                          setup.WebDeploy(wd => wd
                                                    .From.Server(Settings.FromServer, c => c.WithUserName("asdf")
                                                                                               .WithPassword("asdf"))
                                                    .UsingProvider(p => p
                                                                            .WebApp(
                                                                                Settings.WebAppName,
                                                                                Settings.RemoteWebApp,
                                                                                Settings.RemoteWebSite)
                                                    )
                                                    .To.Server(Settings.ToServer));
                          setup.SmokeTest("http://blog.torresdal.net");
								  setup.ApplicationRequestRouting("ffdevnlb01").LoadBalancer.Farm("Sikker").TakeServerOnline("ffdevweb01"); //, a => a.TakeFarmOfflineForServer(Settings.ArrServers.DevServers[Settings.WebServer], Settings.ArrFarmName));
								 //setup.ApplicationRequestRouting("server",
								  //                                arr => arr.TakeFarmOnlineForServer("10.0.0.21", "Farm1"));
                      });
        }
    }
}
