using ConDep.Dsl;

namespace TestWebDeployApp
{
    public class Program : ConDepConsoleApp<Program, ISettings>
    {
        static void Main(string[] args)
        {
            Initialize(args);
        }

        protected override void Execute(ISettings settings)
        {
            Setup(setup =>
                      {
                          setup.PreCompile(settings.AppName, settings.BaseWebPath, settings.PreCompiledWebPath);
                          setup.TransformWebConfig(settings.PreCompiledWebPath, "web.config", "web." + settings.Environment + ".config");

                          setup
                              .ApplicationRequestRouting(settings.ArrServer)
                              .LoadBalancer
                              .Farm(settings.WebFarm)
                              .TakeServerOffline(settings.Servers[0].Ip);

                          setup.Sync(s =>
                                         {
                                             s.From.LocalHost();
                                             s.Using.WebApp(settings.PreCompiledWebPath, settings.AppName,
                                                            settings.WebSiteName);
                                             s.Using.NServiceBus(settings.BaseSvcPath, settings.SvcName,
                                                                 o =>
                                                                 o.DestinationDir(settings.DestSvcPath).UserName(
                                                                     settings.ServiceUser).Password(
                                                                         settings.ServicePassword));
                                             s.To.Server(settings.Servers[0].Name,
                                                         c =>
                                                         c.WithUserName(settings.Servers[0].UserName).WithPassword(
                                                             settings.Servers[0].Password));
                                         }
                              );

                          setup
                              .ApplicationRequestRouting(settings.ArrServer)
                              .LoadBalancer
                              .Farm(settings.WebFarm)
                              .TakeServerOnline(settings.Servers[0].Ip);
                      });
        }
    }
}
