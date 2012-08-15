using ConDep.Dsl;
using ConDep.Dsl.Core;

namespace AssemblyExample
{
    public class MyDeployment : ConDepConfigurator
    {
        protected override void Configure()
        {
            Setup(s =>
                      {
                          s.Infrastructure(infra => infra.IIS.Define(
                              iisDef =>
                                  {
                                      iisDef.AppPool("appPool1", appPoolOpt =>
                                                                     {
                                                                         appPoolOpt.LoadUserProfile = true;
                                                                         appPoolOpt.RecycleTimeIntervalInMinutes = 0;
                                                                     });
                                      iisDef.AppPool("appPool2");

                                      iisDef.WebSite("WebSite1", 2, @"C:\website1",
                                                     webSiteOpt =>
                                                         {
                                                             //webSiteOpt.CopyDir(@"C:\Temp\Frende.Customer.Endpoint2",
                                                             //                   o => o.DestinationDir(@"C:\website1"));
                                                             webSiteOpt.AppPoolName = "appPool1";
                                                             webSiteOpt.HttpBinding(8088);
                                                             webSiteOpt.WebApp("webapp1");
                                                             webSiteOpt.WebApp("webapp2");
                                                             webSiteOpt.WebApp("webapp3");
                                                         });

                                      iisDef.WebSite("WebSite2", 3, @"C:\website2",
                                                     webSiteOpt =>
                                                         {
                                                             webSiteOpt.AppPoolName = "appPool2";
                                                             webSiteOpt.WebApp("webapp4");
                                                         });

                                      iisDef.WebSite("WebSite3", 4, @"C:\website3");
                                  }
                                                        ));
                          //s.Deployment(dep => dep.)
                      });
        }
    }
}   