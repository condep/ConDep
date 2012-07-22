using ConDep.Dsl;
using ConDep.Dsl.Core;

namespace AssemblyExample
{
    public class MyDeployment : ConDepConfigurator
    {
        protected override void Execute()
        {
            Setup(s =>
                      {
// ReSharper disable ConvertToLambdaExpression
                          s.Infrastructure(infra =>
                                               {
                                                   infra.IIS.Define(
                                                       iisDef =>
                                                           {
                                                               iisDef.AppPool("website1", appPoolOpt =>
                                                                                              {
                                                                                                  appPoolOpt.LoadUserProfile = true;
                                                                                                  appPoolOpt.RecycleTimeIntervalInMinutes = 0;
                                                                                              });
                                                               iisDef.AppPool("website2");

                                                               iisDef.WebSite("WebSite1", 2, @"C:\website1",
                                                                              webSiteOpt =>
                                                                                  {
                                                                                      webSiteOpt.AppPoolName = "website1";
                                                                                      webSiteOpt.HttpBinding(8088);
                                                                                      webSiteOpt.WebApp("webapp1");
                                                                                      webSiteOpt.WebApp("webapp2");
                                                                                      webSiteOpt.WebApp("webapp3");
                                                                                  });

                                                               iisDef.WebSite("WebSite2", 3, @"C:\website2",
                                                                              webSiteOpt =>
                                                                                  {
                                                                                      webSiteOpt.AppPoolName = "website2";
                                                                                      webSiteOpt.WebApp("webapp1");
                                                                                  });

                                                               iisDef.WebSite("WebSite3", 4, @"C:\website3");

                                                           }
                                                       );

                                                   //infra.CopyCertificate();
                                               } 
                            );

                          s.Deployment(dep =>
                                           {
                                               dep.CopyDir(@"C:\Temp\Frende.Customer.Endpoint2", o => o.DestinationDir(@"C:\website1"));
                                           });
// ReSharper restore ConvertToLambdaExpression

                          //s.Deployment(deploy => deploy.CopyDir());
                      }
                  );
        }
    }
}   