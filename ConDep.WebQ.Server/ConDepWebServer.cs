using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace ConDep.WebQ.Server
{
    internal sealed class ConDepWebServer : IDisposable
    {
        private readonly int _port;
        private readonly EventLog _eventLog;
        private HttpListener _listener;

        private bool _disposed;
        private const string PREFIX = "http://+:{0}/ConDepWebQ/";
        private readonly HttpProcessHandler _processHandler;

        public ConDepWebServer(int port, int timeout, EventLog eventLog)
        {
            _port = port;
            _eventLog = eventLog;
            _processHandler = new HttpProcessHandler(eventLog, timeout);
        }

        public void Start()
        {
            if (!HttpListener.IsSupported)
            {
                _eventLog.WriteEntry("ConDepWebQ cannot start. OS not supported.", EventLogEntryType.Error);
                Environment.Exit(1);
            }

            _listener = new HttpListener();
            _listener.Prefixes.Add(string.Format(PREFIX, _port));
            _listener.Start();

            var timer = new Timer(60000);
            timer.Elapsed += _processHandler.RemoveTimedOutItems;
            timer.Start();

            _listener.BeginGetContext(HandleRequest, _listener);
        }

        public void Stop()
        {
            if(_listener != null)
            {
                if(_listener.IsListening)
                {
                    _listener.Stop();
                }
            }
        }

        private void HandleRequest(IAsyncResult result)
        {
            HttpListenerContext context = null;
            try
            {
                var listener = (HttpListener) result.AsyncState;
                context = listener.EndGetContext(result);
                Task.Factory.StartNew(() =>
                                          {
                                              var statusCode = _processHandler.ProcessRequest(context);
                                              context.Response.StatusCode = (int)statusCode;
                                              context.Response.OutputStream.Close();
                                          });
                listener.BeginGetContext(HandleRequest, listener);
            }
            catch(Exception ex)
            {
                _eventLog.WriteEntry("An unhandled exception has occurred:\n\n" + ex, EventLogEntryType.Error);
                if (context == null) return;
                
                if(context.Response.OutputStream != null)
                {
                    try
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.OutputStream.Close();
                    }
                    catch { _eventLog.WriteEntry("Unable to return HTTP status after exception during request processing.", EventLogEntryType.Error); }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if(_listener != null)
                    {
                        _listener.Close();
                    }
                }
                _disposed = true;
            }
        }
    }
}