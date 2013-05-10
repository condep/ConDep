using System;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConDep.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:80/ConDepNode/");
            var config = new HttpSelfHostConfiguration(uri)
                             {
                                 TransferMode = TransferMode.Streamed,
                                 MaxReceivedMessageSize = 2147483648
                                 //MaxReceivedMessageSize = 2000000
                             };

            var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute("Sync", "api/sync/{controller}");
            config.Routes.MapHttpRoute("WebAppSync", "api/sync/webapp/{siteName}/{appName}", new { controller = "WebApp" });
            config.Routes.MapHttpRoute("Iis", "api/iis/{siteName}/{appName}", new { controller = "Iis", siteName = RouteParameter.Optional, appName = RouteParameter.Optional });
            config.Routes.MapHttpRoute("Api", "api/{controller}", new { controller = "Home" });
            //config.Routes.MapHttpRoute("Iis", "api/{controller}", new { controller = "Iis" });
            //config.Routes.MapHttpRoute("WebApp", "api/iis/{controller}", new { controller = "IisWebApp" });
            //config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            //config.Routes.MapHttpRoute("File Upload", "api/sync/{controller}/{filename}");

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }

    //internal class ClientProcessHandler : IProcessHttpCalls
    //{
    //    public HttpStatusCode ProcessRequest(HttpListenerContext context)
    //    {
    //        if (context.Request.HttpMethod.ToUpper() == "PUT")
    //        {
    //            var fileName = context.Request.Url.Segments[2];
    //            using (var output = File.Create(@"C:\temp\" + fileName))
    //            {
    //                context.Request.InputStream.CopyTo(output);
    //            }

    //            return HttpStatusCode.Created;
    //        }
    //        else
    //        {
    //            string message = "Hello there!!! :-)";
    //            var bytes = Encoding.UTF8.GetBytes(message);
    //            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
    //            return HttpStatusCode.OK;
    //        }
    //    }

    //    public void RemoveTimedOutItems(object sender, ElapsedEventArgs e)
    //    {

    //    }
    //}
}
