using System;

namespace ConDep.WebQ.Client
{
    public class WebQueueEventArgs : EventArgs
    {
        private string _message;

        public WebQueueEventArgs(string message)
        {
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }
    }
}