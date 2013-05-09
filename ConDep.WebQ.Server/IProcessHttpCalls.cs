using System.Net;
using System.Timers;

namespace ConDep.WebQ.Server
{
    public interface IProcessHttpCalls
    {
        HttpStatusCode ProcessRequest(HttpListenerContext context);
        void RemoveTimedOutItems(object sender, ElapsedEventArgs e);
    }
}