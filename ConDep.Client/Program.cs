using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Xml.Serialization;
using ConDep.WebQ.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

//using HttpStatusCode = ConDep.WebQ.Server.HttpStatusCode;

//using HttpStatusCode = ConDep.WebQ.Server.HttpStatusCode;

namespace ConDep.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = 80;
            string prefix = "http://+:{0}/ConDepClient/";

            //var processHandler = new ClientProcessHandler();
            //if(!EventLog.Exists("ConDepClient"))
            //{
            //    EventLog.CreateEventSource("Application", "ConDepClient");
            //}
            //var eventLog = new EventLog("Application", ".", "ConDepClient");
            //using(var webServer = new ConDepWebServer(port, eventLog, processHandler, prefix))
            //{
            //    webServer.Start();
            //    while (true) { Thread.Sleep(100); }
            //}
            //webServer.Start();

            var config = new HttpSelfHostConfiguration(string.Format("http://localhost:{0}/ConDepClient/", port))
                             {TransferMode = TransferMode.Streamed, MaxReceivedMessageSize = 2147483648};

            var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("Sync", "api/sync/{controller}");
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
