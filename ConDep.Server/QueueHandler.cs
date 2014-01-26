using System;
using System.Collections.Generic;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Model.QueueAggregate;
using Raven.Client;

namespace ConDep.Server
{
    public class QueueHandler
    {
        private readonly IDocumentSession _session;
        private readonly Dictionary<string, QueueItem> _itemsInExecution;

        public QueueHandler(IDocumentSession session, Dictionary<string, QueueItem> itemsInExecution)
        {
            _session = session;
            _itemsInExecution = itemsInExecution;
        }

        public void RemoveFromQueue(string id, ConDepExecutionResult result)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var item = _session.Load<QueueItem>(RavenDb.GetFullId<QueueItem>(id));
            //    item.FinishedUtc = DateTime.UtcNow;
            //    item.QueueStatus = result.Cancelled ? QueueStatus.Cancelled : QueueStatus.Finished;

            //    _session.SaveChanges();
            //    if(_itemsInExecution.ContainsKey(item.Id.ToString())) _itemsInExecution.Remove(item.Id.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw new ConDepQueueException("Unable to to remove item from queue. See inner exception for details.",
            //                                   ex);
            //}
        }
    }
}