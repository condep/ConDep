using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using WebApi.Hal;
using Link = ConDep.Node.Model.Link;

namespace ConDep.Node.Controllers
{
    public class ApiRepresentation : Representation
    {
        private readonly UrlHelper _urlHelper;

        public ApiRepresentation(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        protected override void CreateHypermedia()
        {
            Href = ApiUrls.Home(_urlHelper);
            Rel = "self";
            Links.Add(new WebApi.Hal.Link("asdf", "asdf"));
        }
    }

    public class HomeController : ApiController
    {
        public IEnumerable<Link> Get()
         {
             return new List<Link>
                        {
                            new Link { Href = ApiUrls.Home(Url), Rel = "self" },
                            new Link { Href = ApiUrls.Sync.Home(Url), Rel = "http://www.con-dep.net/rels/sync"},
                            new Link {Href = ApiUrls.Sync.DirectoryTemplate(Url), Rel = "http://www.con-dep.net/rels/sync/dir_template"},
                            new Link {Href = ApiUrls.Sync.FileTemplate(Url), Rel = "http://www.con-dep.net/rels/sync/file_template"}
                        };
             //return new dynamic[]
             //           {
             //               new Link {Href = ApiUrls.Home(Url), Rel = "self"},
             //               new List<Link>
             //                   {
             //                       new Link {Href = ApiUrls.Sync.Directory(Url), Rel = "self"},
             //                       new Link {Href = ApiUrls.Sync.File(Url), Rel = "self"},
             //                   }
             //           };

         }
    }
}