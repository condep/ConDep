using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ConDep.WebQ.Server
{
    internal class ConDepWebServer : IDisposable
    {
        private readonly EventLog _eventLog;
        private HttpListener _listener;

        private bool _disposed;
        private const string _prefix = "http://+:80/ConDepWebQ/";
        private HttpProcessHandler _processHandler;

        public ConDepWebServer(int timeout, EventLog eventLog)
        {
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
            _listener.Prefixes.Add(_prefix);
            _listener.Start();

            var timer = new Timer(10000);
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
            var listener = (HttpListener) result.AsyncState;
            var context = listener.EndGetContext(result);
            Task.Factory.StartNew(() => _processHandler.ProcessRequest(context));
            listener.BeginGetContext(HandleRequest, listener);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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