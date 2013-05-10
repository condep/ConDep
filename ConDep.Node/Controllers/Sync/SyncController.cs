using System.Collections.Generic;
using System.Web.Http;
using ConDep.Node.Model;

namespace ConDep.Node.Controllers.Sync
{
    public class SyncController : ApiController
    {
         public IEnumerable<Link> Get()
         {
             return new[]
                        {
                            new Link {Href = ApiUrls.Sync.Directory(Url), Rel = ApiRels.Self},
                            new Link {Href = ApiUrls.Sync.FileTemplate(Url), Rel = ApiRels.Self},
                        };
         }
    }
}