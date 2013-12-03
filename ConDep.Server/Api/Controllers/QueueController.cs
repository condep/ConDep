using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using ConDep.Server.Api.Model;

namespace ConDep.Server.Api.Controllers
{
    public class QueueController : ApiController
    {
         public List<QueueItem> Get()
         {
             using (var session = RavenDb.DocumentStore.OpenSession())
             {
                 return session.Query<QueueItem>()
                                                .Where(x => x.QueueStatus != QueueStatus.Finished)
                                                .OrderBy(order => order.CreatedUtc).ToList();
             }
         }
    }
}