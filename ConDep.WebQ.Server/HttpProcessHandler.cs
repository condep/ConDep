using System;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace ConDep.WebQ.Server
{
    public enum HttpStatusCode
    {
        OK = 200,
        Created = 201,
        NoContent = 204,
        NotFound = 404,
        InternalServerError = 500
    }

    public class HttpProcessHandler : IProcessHttpCalls
    {
        private readonly EventLog _eventLog;
        private readonly int _queueItemTimeout;
        private readonly WebQ _queue;

        public HttpProcessHandler(EventLog eventLog, int queueItemTimeout)
        {
            _eventLog = eventLog;
            _queueItemTimeout = queueItemTimeout;
            _queue = new WebQ(eventLog);
        }

        public HttpStatusCode ProcessRequest(HttpListenerContext context)
        {
            lock (_queue.AsyncRoot)
            {
                try
                {
                    if (context.Request.Url.Segments.Length < 2 || context.Request.Url.Segments.Length > 4)
                    {
                        return HttpStatusCode.NotFound;
                    }

                    if (context.Request.Url.Segments.Length == 2)
                    {
                        return ProcessRoot(context);
                    }

                    var environment = context.Request.Url.Segments[2].TrimEnd('/');

                    if (context.Request.Url.Segments.Length == 3)
                    {
                        return ProcessEnvironment(context, environment);
                    }

                    if (context.Request.Url.Segments.Length == 4)
                    {
                        return ProcessItemId(context, environment);
                    }

                    return HttpStatusCode.NotFound;
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message);
                    return HttpStatusCode.InternalServerError;
                }
            }
        }

        private HttpStatusCode ProcessRoot(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.ToUpper() == "GET")
            {
                return GetQueue(context);
            }
            if (context.Request.HttpMethod.ToUpper() == "DELETE")
            {
                return ClearQueue();
            }

            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode ProcessEnvironment(HttpListenerContext context, string environment)
        {
            if (context.Request.HttpMethod.ToUpper() == "GET")
            {
                return GetEnvironmentQueue(environment, context);
            }
            if (context.Request.HttpMethod.ToUpper() == "PUT")
            {
                return AddToQueue(environment, context);
            }
            if (context.Request.HttpMethod.ToUpper() == "DELETE")
            {
                return ClearQueue(environment, context);
            }

            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode ProcessItemId(HttpListenerContext context, string environment)
        {
            var id = context.Request.Url.Segments[3].TrimEnd('/');

            switch (context.Request.HttpMethod.ToUpper())
            {
                case "POST":
                    return TagAsStarted(environment, id, context);
                case "DELETE":
                    return RemoveFromQueue(environment, id, context);
                case "GET":
                    return GetItem(environment, id, context);
            }
            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode ClearQueue()
        {
            _queue.Clear();
            return HttpStatusCode.NoContent; 
        }

        private HttpStatusCode ClearQueue(string environment, HttpListenerContext context)
        {
            if (_queue.TryDequeue(environment))
            {
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode TagAsStarted(string environment, string id, HttpListenerContext context)
        {
            var item = _queue.Poke(environment, id);
            if (item != null)
            {
                return ConDepReturnCodes.Created(context, item);
            }
            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode GetItem(string environment, string id, HttpListenerContext context)
        {
            var item = _queue.Peek(environment, id);
            if (item != null)
            {
                return ConDepReturnCodes.Found(context, item);
            }
            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode GetEnvironmentQueue(string environment, HttpListenerContext context)
        {
            var envQ = _queue.Peek(environment);
            if (envQ == null)
            {
                return HttpStatusCode.NotFound;
            }
            return ConDepReturnCodes.Found(context, envQ);
        }

        private HttpStatusCode AddToQueue(string environment, HttpListenerContext context)
        {
            var item = _queue.Enqueue(environment);
            return ConDepReturnCodes.Created(context, item);
        }

        private HttpStatusCode GetQueue(HttpListenerContext context)
        {
            var webQData = _queue.Peek();
            if(webQData != null)
            {
                return ConDepReturnCodes.Found(context, webQData);
            }
            return HttpStatusCode.NotFound;
        }

        private HttpStatusCode RemoveFromQueue(string environment, string id, HttpListenerContext context)
        {
            if (_queue.TryDequeue(environment, id))
            {
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.NotFound;
        }

        public void RemoveTimedOutItems(object sender, ElapsedEventArgs e)
        {
            _queue.DequeueTimedOut(_queueItemTimeout);
        }
 
    }
}