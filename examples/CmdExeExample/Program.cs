using ConDep.Dsl.FluentWebDeploy;

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
            Sync(s => s
                          .From.Server(Settings.FromServer, c => c
									  .WithUserName("asdf")
									  .WithPassword("asdf"))
                          .UsingProvider(p => p.WebApp(Settings.WebAppName, Settings.RemoteWebApp, Settings.RemoteWebSite))
                          .To.Server(Settings.ToServer));
        }
    }
}
