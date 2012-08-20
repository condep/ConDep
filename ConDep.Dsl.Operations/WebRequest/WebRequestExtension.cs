using ConDep.Dsl.Core;
using ConDep.Dsl.Operations.WebRequest;

namespace ConDep.Dsl
{
    public static class WebRequestExtension
    {
         public static void WebRequest(this ISetupCondep conDepSetup, string method, string url)
         {
             ((ConDepSetup)conDepSetup).AddOperation(new WebRequestOperation(url, method));
         }
    }
}