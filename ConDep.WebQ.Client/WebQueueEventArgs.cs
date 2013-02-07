using System;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Client
{
    public class WebQueueEventArgs : EventArgs
    {
        private string _message;
        private readonly WebQItem _item;

        public WebQueueEventArgs(string message, WebQItem item)
        {
            _message = message;
            _item = item;
        }

        public string Message
        {
            get { return _message; }
        }

        public WebQItem Item
        {
            get { return _item; }
        }
    }
}