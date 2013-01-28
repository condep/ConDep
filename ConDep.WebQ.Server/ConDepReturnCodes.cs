using System.Net;
using System.Runtime.Serialization.Json;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Server
{
    internal static class ConDepReturnCodes
    {
        public static void NotFound(HttpListenerContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "Item not found in queue";
            context.Response.OutputStream.Close();
        }

        public static void Created(HttpListenerContext context, WebQItem item)
        {
            context.Response.StatusCode = 201;
            context.Response.StatusDescription = "WebQ Item created";

            var serializer = new DataContractJsonSerializer(item.GetType());
            serializer.WriteObject(context.Response.OutputStream, item);

            context.Response.OutputStream.Close();

        }

        public static void NoContent(HttpListenerContext context)
        {
            context.Response.StatusCode = 204;
            context.Response.OutputStream.Close();
        }

        public static void Found(HttpListenerContext context, WebQItem item)
        {
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "WebQ Item found";

            var serializer = new DataContractJsonSerializer(item.GetType());
            serializer.WriteObject(context.Response.OutputStream, item);

            context.Response.OutputStream.Close();
        }

        public static void InternalServerError(HttpListenerContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.OutputStream.Close();
        }
    }
}