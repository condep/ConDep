using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Timers;

namespace ConDep.WebQ.Server
{
    public class HttpProcessHandler
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

        public void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                if (context.Request.Url.Segments.Length < 2 || context.Request.Url.Segments.Length > 4)
                {
                    ConDepReturnCodes.NotFound(context);
                    return;
                }

                if (context.Request.Url.Segments.Length == 2)
                {
                    if (context.Request.HttpMethod.ToUpper() == "GET")
                    {
                        GetQueue(context);
                        return;
                    }
                    if (context.Request.HttpMethod.ToUpper() == "DELETE")
                    {
                        DeleteQueue(context);
                        return;
                    }

                    ConDepReturnCodes.NotFound(context);
                    return;

                }

                var environment = context.Request.Url.Segments[2].TrimEnd('/');

                if (context.Request.Url.Segments.Length == 3)
                {
                    if (context.Request.HttpMethod.ToUpper() == "GET")
                    {
                        GetEnvironmentQueue(environment, context);
                        return;
                    }
                    if (context.Request.HttpMethod.ToUpper() == "PUT")
                    {
                        AddToQueue(environment, context);
                        return;
                    }
                    if (context.Request.HttpMethod.ToUpper() == "DELETE")
                    {
                        DeleteQueue(environment, context);
                        return;
                    }

                    ConDepReturnCodes.NotFound(context);
                    return;
                }

                if (context.Request.Url.Segments.Length == 4)
                {
                    var id = context.Request.Url.Segments[3].TrimEnd('/');

                    switch (context.Request.HttpMethod.ToUpper())
                    {
                        case "POST":
                            StartItem(environment, id, context);
                            return;
                        case "DELETE":
                            RemoveFromQueue(environment, id, context);
                            return;
                        case "GET":
                            GetItem(environment, id, context);
                            return;
                    }
                    ConDepReturnCodes.NotFound(context);
                    return;
                }

                ConDepReturnCodes.NotFound(context);
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.Message);
                ConDepReturnCodes.InternalServerError(context);
            }
        }

        private void DeleteQueue(HttpListenerContext context)
        {
            _queue.Clear();
            ConDepReturnCodes.NoContent(context);
        }

        private void DeleteQueue(string environment, HttpListenerContext context)
        {
            if (_queue.TryRemove(environment))
            {
                ConDepReturnCodes.NoContent(context);
                return;
            }
            ConDepReturnCodes.NotFound(context);
        }

        private void StartItem(string environment, string id, HttpListenerContext context)
        {
            var item = _queue.Get(environment, id);
            if (item != null)
            {
                item.StartTime = DateTime.Now;
                ConDepReturnCodes.Created(context, item);
            }
            else
            {
                ConDepReturnCodes.NotFound(context);
            }
        }

        private void GetItem(string environment, string id, HttpListenerContext context)
        {
            var item = _queue.Get(environment, id);
            if (item != null)
            {
                ConDepReturnCodes.Found(context, item);
            }
            else
            {
                ConDepReturnCodes.NotFound(context);
            }
        }

        private void GetEnvironmentQueue(string environment, HttpListenerContext context)
        {
            var envQ = _queue.Get(environment);
            if (envQ == null)
            {
                ConDepReturnCodes.NotFound(context);
            }
            else
            {
                var serializer = new DataContractJsonSerializer(envQ.GetType());
                serializer.WriteObject(context.Response.OutputStream, envQ);
                context.Response.OutputStream.Close();
            }
        }

        private void AddToQueue(string environment, HttpListenerContext context)
        {
            var item = _queue.Add(environment);
            ConDepReturnCodes.Created(context, item);
        }

        private void GetQueue(HttpListenerContext context)
        {
            var webQData = _queue.Get();
            var serializer = new DataContractJsonSerializer(webQData.GetType());
            serializer.WriteObject(context.Response.OutputStream, webQData);
            context.Response.OutputStream.Close();
        }

        private void RemoveFromQueue(string environment, string id, HttpListenerContext context)
        {
            if (!_queue.Exist(environment, id))
            {
                ConDepReturnCodes.NotFound(context);
                return;
            }

            if (_queue.TryRemove(environment, id))
            {
                ConDepReturnCodes.NoContent(context);
            }
            else
            {
                ConDepReturnCodes.NotFound(context);
            }
        }

        public void RemoveTimedOutItems(object sender, ElapsedEventArgs e)
        {
            _queue.RemoveOldEntries(_queueItemTimeout);
        }
 
    }
}