using System;
using System.Collections.Generic;
using ConDep.Server.Model.QueueAggregate;

namespace ConDep.Server.Api.Controllers
{
    public class QueueController : RavenDbController
    {
         public List<QueueItem> Get()
         {
                 //return Session.Query<QueueItem, QueueItem_ByEnvironmentAndStatus>()
                 //                               .Where(x => x.QueueStatus != QueueStatus.Finished)
                 //                               .OrderBy(order => order.CreatedUtc).ToList();
             throw new NotImplementedException();
         }
    }
}