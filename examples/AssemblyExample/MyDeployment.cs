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
                                                               iisDef.WebSite("agent.frende.no", 2,
                                                                              @"C:\agent.frende.no",
                                                                              webSiteOpt =>
                                                                                  {
                                                                                      webSiteOpt.HttpBinding(8088);
                                                                                      webSiteOpt.ApplicationPool(
                                                                                          "agent.frende.no");
                                                                                      webSiteOpt.WebApp("Finansportalen");
                                                                                      webSiteOpt.WebApp("Front");
                                                                                      webSiteOpt.WebApp("STS");
                                                                                  });

                                                               iisDef.WebSite("sikker.frende.no", 3,
                                                                              @"C:\sikker.frende.no",
                                                                              webSiteOpt =>
                                                                                  {
                                                                                      webSiteOpt.ApplicationPool(
                                                                                          "sikker.frende.no");
                                                                                      webSiteOpt.WebApp("Selvbetjent");
                                                                                  });

                                                               iisDef.WebSite("tjenester.frende.no", 4, @"C:\tjenester.frende.no");
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