using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebRequest;

namespace ConDep.Dsl
{
    public static class WebRequestExtension
    {
         public static void WebRequest(this DeploymentOptions options, string method, string url)
         {
             options.AddOperation(new WebRequestOperation(url, method));
         }
    }
}