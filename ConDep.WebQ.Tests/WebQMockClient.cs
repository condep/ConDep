using System;
using ConDep.WebQ.Client;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Tests
{
    public class WebQMockClient : IClient
    {
        private readonly int _startPosition;

        public WebQMockClient(int startPosition)
        {
            _startPosition = startPosition;
        }

        public WebQItem Enqueue(string environment)
        {
            return new WebQItem{CreatedTime = DateTime.Now, Environment = environment, Id = Guid.NewGuid().ToString(), Position = _startPosition};
        }

        public WebQItem Peek(WebQItem item)
        {
            item.Position--;
            return item;
        }

        public void Dequeue(WebQItem item)
        {
            
        }

        public WebQItem SetAsStarted(WebQItem item)
        {
            item.StartTime = DateTime.Now;
            return item;
        }
    }
}