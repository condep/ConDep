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
                          setup.TransformWebConfig(@"C:\Temp\MyApp", "Test");
                          setup.PreCompile("MyApp", @"C:\Temp\MyApp", @"C:\temp\MyApp2");
                          setup.ApplicationRequestRouting("server",
                                                          arr => arr.TakeFarmOfflineForServer("10.0.0.21", "Farm1"));
                          setup.WebDeploy(wd => wd
                                                    .From.Server(Settings.FromServer, c => c
                                                                                               .WithUserName("asdf")
                                                                                               .WithPassword("asdf"))
                                                    .UsingProvider(p => p
                                                                            .WebApp(
                                                                                Settings.WebAppName,
                                                                                Settings.RemoteWebApp,
                                                                                Settings.RemoteWebSite)
                                                    )
                                                    .To.Server(Settings.ToServer));
                          setup.SmokeTest("http://blog.torresdal.net");
                          setup.ApplicationRequestRouting("server",
                                                          arr => arr.TakeFarmOnlineForServer("10.0.0.21", "Farm1"));
                      });
        }
    }
}
