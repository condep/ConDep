using System;
using System.Globalization;
using System.Threading;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Client
{
    public class WebQueue
    {
        private readonly string _environment;
        private readonly TimeSpan _refreshInterval;
        private readonly IClient _client;
        private WebQItem _item;

        public event EventHandler<WebQueueEventArgs> WebQueuePositionUpdate;
        public event EventHandler<WebQueueEventArgs> WebQueueTimeoutUpdate;

        public WebQueue(string webQAddress, string environment)
        {
            _client = new Client(new Uri(webQAddress));
            _environment = environment;
            _refreshInterval = TimeSpan.FromSeconds(10);
        }

        public WebQueue(IClient client, string environment, TimeSpan refreshInterval)
        {
            _client = client;
            _environment = environment;
            _refreshInterval = refreshInterval;
        }

        protected virtual void OnWebQueuePositionUpdate(object sender, WebQueueEventArgs args)
        {
            if(WebQueuePositionUpdate != null)
            {
                WebQueuePositionUpdate(sender, args);
            }
        }

        protected virtual void OnWebQueueTimeoutUpdate(object sender, WebQueueEventArgs args)
        {
            if (WebQueueTimeoutUpdate != null)
            {
                WebQueueTimeoutUpdate(sender, args);
            }
        }

        public void WaitInQueue(TimeSpan timeout)
        {
            const string positionMessage = "Waiting in deployment queue. There are {0} deployment(s) ahead of you.";
            var timeForTimeout = DateTime.Now + timeout;

            _item = _client.Enqueue(_environment);
            OnWebQueuePositionUpdate(this, new WebQueueEventArgs(string.Format(positionMessage, _item.Position), _item));

            if (_item.Position == 0)
            {
                _client.SetAsStarted(_item);
                return;
            }

            var currentPosition = _item.Position;
            do
            {
                Thread.Sleep(_refreshInterval);
                _item = _client.Peek(_item);

                if(currentPosition != _item.Position)
                {
                    currentPosition = _item.Position;
                    if (currentPosition == 0) break;
                    OnWebQueuePositionUpdate(this, new WebQueueEventArgs(string.Format(positionMessage, currentPosition), _item));
                }

                if (DateTime.Now > timeForTimeout)
                {
                    try
                    {
                        _client.Dequeue(_item);
                    }
                    catch
                    {
                        throw new TimeoutException("ConDep timed out waiting in queue and failed to remove itself from the queue.");
                    }
                    throw new TimeoutException("ConDep timed out waiting in queue.");
                }

                OnWebQueueTimeoutUpdate(this, new WebQueueEventArgs(string.Format("Will wait in queue for {0} minutes before timing out.", (timeForTimeout - DateTime.Now).ToString("c", CultureInfo.CurrentCulture)), _item));
            } while (true);

            _item = _client.SetAsStarted(_item);
        }

        public void LeaveQueue()
        {
            _client.Dequeue(_item);
        }
 
    }
}