using ConDep.Dsl;

namespace AssemblyExample
{
    public class MyDeployment : ConDepConfiguratorBase
    {
        protected override void Configure()
        {
            Setup(s =>
                      {
                          s.Infrastructure(inf =>
                                               {
                                                   inf.RunCmd("ipconfig");
                                                   inf.RunCmd("echo 'Hello World!'");
                                               });
                          s.ConDepContext("MyWebApp", SetupMyWebApp );
                          s.ConDepContext("MyOtherApp", SetupMyOtherApp );
                          //todo: 
                          s.Infrastructure(inf =>
                          {
                              inf.RunCmd("echo 'This should also be executed!'");
                          });
                      });
        }

        private void SetupMyWebApp(IProvideForSetup s)
        {
            s.Infrastructure(infra => infra.Iis(
                iis =>
                {
                    iis.AppPool("appPool1", appPoolOpt =>
                    {
                        appPoolOpt.LoadUserProfile = true;
                        appPoolOpt.RecycleTimeIntervalInMinutes = 0;
                    });
                    iis.WebSite("WebSite1", 2, @"C:\website1",
                                   webSiteOpt =>
                                   {
                                       webSiteOpt.AppPoolName = "appPool1";
                                       webSiteOpt.WebApp("webapp1", webAppOpt =>
                                       {
                                           webAppOpt.ApplicationPool = "appPool2";
                                       });
                                       webSiteOpt.WebApp("webapp2");
                                       webSiteOpt.WebApp("webapp3");
                                   });

                    //iis.WebSite("WebSite2", 3, @"C:\website2",
                    //               webSiteOpt =>
                    //               {
                    //                   webSiteOpt.AppPoolName = "appPool2";
                    //                   webSiteOpt.WebApp("webapp4");
                    //               });

                    //iis.WebSite("WebSite3", 4, @"C:\website3");

                    //iis.WebApp("Webapp1", "WebSite3");
                }
                                          ));
        }

        private void SetupMyOtherApp(IProvideForSetup s)
        {
            s.Infrastructure(infra => infra.Iis(
                iis =>
                    {
                        iis.AppPool("appPool2");
                        iis.WebSite("WebSite2", 3, @"C:\website2",
                                    webSiteOpt =>
                                        {
                                            webSiteOpt.AppPoolName = "appPool2";
                                            webSiteOpt.WebApp("webapp4");
                                        });
                    }
                                          ));
        }
    }
}   