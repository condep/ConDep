using System;
using System.Net;
using System.Runtime.Serialization.Json;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Client
{
    public class Proxy
    {
        private readonly Uri _webQAddress;
        private bool? _waitingInQueue;

        public Proxy(Uri webQAddress)
        {
            _webQAddress = webQAddress;
        }

        public WebQItem AddToQueue(string environment)
        {
            var request = WebRequest.Create(new Uri(_webQAddress, environment));
            request.Method = "PUT";
            request.ContentLength = 0;
            var response = request.GetResponse();

            var serializer = new DataContractJsonSerializer(typeof(WebQItem));
            var stream = response.GetResponseStream();
            if (stream != null)
            {
                _waitingInQueue = true;
                return serializer.ReadObject(stream) as WebQItem;
            }
            return new WebQItem {Position = -1};
        }

        public WebQItem Update(WebQItem item)
        {
            var request = WebRequest.Create(new Uri(_webQAddress, item.Environment + "/" + item.Id));
            request.Method = "GET";
            request.ContentLength = 0;
            var response = request.GetResponse();

            var serializer = new DataContractJsonSerializer(typeof(WebQItem));
            var stream = response.GetResponseStream();
            if (stream != null)
            {
                return serializer.ReadObject(stream) as WebQItem;
            }
            return item;
        }

        public void RemoveFromQueue(WebQItem item)
        {
            if (!_waitingInQueue.HasValue) return;
            if (!_waitingInQueue.Value) return;

            var request = WebRequest.Create(new Uri(_webQAddress, item.Environment + "/" + item.Id));
            request.Method = "DELETE";
            request.ContentLength = 0;
            request.GetResponse();
            _waitingInQueue = false;
        }

        public WebQItem Alert(WebQItem item)
        {
            var request = WebRequest.Create(new Uri(_webQAddress, item.Environment + "/" + item.Id));
            request.Method = "POST";
            request.ContentLength = 0;
            var response = request.GetResponse();

            var serializer = new DataContractJsonSerializer(typeof(WebQItem));
            var stream = response.GetResponseStream();
            if (stream != null)
            {
                return serializer.ReadObject(stream) as WebQItem;
            }
            return item;
        }
    }
}