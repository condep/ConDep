using System.Collections.Generic;
using System.Web.Http;
using ConDep.Node.Model;

namespace ConDep.Node.Controllers
{
    public class SyncController : ApiController
    {
         public IEnumerable<Link> Get()
         {
             return new[]
                        {
                            new Link {Href = ApiUrls.Sync.Directory(Url), Rel = "self"},
                            new Link {Href = ApiUrls.Sync.FileTemplate(Url), Rel = "self"},
                        };
         }
    }
}