using ConDep.Dsl;
using ConDep.Dsl.Operations.WebRequest;

namespace ConDep.Dsl
{
    public static class WebRequestExtension
    {
        public static void WebRequest(this IProvideForSetup conDepSetup, string method, string url)
         {
             ((ConDepSetup)conDepSetup).AddOperation(new WebRequestOperation(url, method));
         }
    }
}