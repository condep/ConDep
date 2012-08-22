using ConDep.Dsl;
using ConDep.Dsl.Core;

namespace AssemblyExample
{
    public class MyDeployment : ConDepConfiguratorBase
    {
        protected override void Configure()
        {
            Setup(s =>
                      {
// ReSharper disable ConvertToLambdaExpression
                          s.Infrastructure(infra => infra.Iis(
                              iis =>
                                  {
                                      iis.AppPool("appPool1", appPoolOpt =>
                                                                     {
                                                                         appPoolOpt.LoadUserProfile = true;
                                                                         appPoolOpt.RecycleTimeIntervalInMinutes = 0;
                                                                     });
                                      iis.AppPool("appPool2");

                                      iis.WebSite("WebSite1", 2, @"C:\website1",
                                                     webSiteOpt =>
                                                         {
                                                             //webSiteOpt.CopyDir(@"C:\Temp\Frende.Customer.Endpoint2",
                                                             //                   o => o.DestinationDir(@"C:\website1"));
                                                             webSiteOpt.AppPoolName = "appPool1";
                                                             webSiteOpt.HttpBinding(8088);
                                                             webSiteOpt.WebApp("webapp1", webAppOpt =>
                                                                                              {
                                                                                                  webAppOpt.ApplicationPool = "appPool2";
                                                                                              });
                                                             webSiteOpt.WebApp("webapp2");
                                                             webSiteOpt.WebApp("webapp3");
                                                         });

                                      iis.WebSite("WebSite2", 3, @"C:\website2",
                                                     webSiteOpt =>
                                                         {
                                                             webSiteOpt.AppPoolName = "appPool2";
                                                             webSiteOpt.WebApp("webapp4");
                                                         });

                                      iis.WebSite("WebSite3", 4, @"C:\website3");

                                      iis.WebApp("Webapp1", "WebSite3");
                                  }
                                                        ));
// ReSharper restore ConvertToLambdaExpression

                          //s.Deployment(dep => dep.)
                      });
        }
    }
}   