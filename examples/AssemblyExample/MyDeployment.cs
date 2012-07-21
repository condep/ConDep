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
                                                               iisDef.WebSite("WebSite1", 2,
                                                                              @"C:\website1",
                                                                              webSiteOpt =>
                                                                                  {
                                                                                      webSiteOpt.HttpBinding(8088);
                                                                                      webSiteOpt.ApplicationPool(
                                                                                          "website1");
                                                                                      webSiteOpt.WebApp("webapp1");
                                                                                      webSiteOpt.WebApp("webapp2");
                                                                                      webSiteOpt.WebApp("webapp3");
                                                                                  });

                                                               iisDef.WebSite("WebSite2", 3,
                                                                              @"C:\website2",
                                                                              webSiteOpt =>
                                                                                  {
                                                                                      webSiteOpt.ApplicationPool(
                                                                                          "website2");
                                                                                      webSiteOpt.WebApp("webapp1");
                                                                                  });

                                                               iisDef.WebSite("WebSite3", 4, @"C:\website3");
                                                           }
                                                       );
                                               } 
                            );
// ReSharper restore ConvertToLambdaExpression

                          //s.Deployment(deploy => deploy.CopyDir());
                      }
                  );
        }
    }
}   