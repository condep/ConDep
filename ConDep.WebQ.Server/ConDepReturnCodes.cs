using System.Net;
using System.Runtime.Serialization.Json;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Server
{
    internal static class ConDepReturnCodes
    {
        public static HttpStatusCode Created(HttpListenerContext context, WebQItem item)
        {
            var serializer = new DataContractJsonSerializer(item.GetType());
            serializer.WriteObject(context.Response.OutputStream, item);
            return HttpStatusCode.Created;
        }

        public static HttpStatusCode Found(HttpListenerContext context, object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            serializer.WriteObject(context.Response.OutputStream, obj);
            return HttpStatusCode.OK;
        }
    }
}