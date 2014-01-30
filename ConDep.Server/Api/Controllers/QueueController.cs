using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Server.Domain.Queue.Model;

namespace ConDep.Server.Api.Controllers
{
    public class QueueController : RavenDbController
    {
         public List<QueueItem> Get()
         {
             return Session.Query<QueueItem>().ToList();
         }
    }
}