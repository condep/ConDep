using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Node.Model;
using Microsoft.Web.Administration;

namespace ConDep.Node.Controllers.Sync
{
    public class WebAppController : ApiController
    {
        private readonly PathValidator _pathValidator;

        public WebAppController()
            : this(new PathValidator())
        {
        }

        public WebAppController(PathValidator pathValidator)
        {
            _pathValidator = pathValidator;
        }

        public HttpResponseMessage Get(string siteName, string appName)
        {
            var webAppInfo = new WebAppInfo();

            var manager = new ServerManager();
            var site = manager.Sites[siteName];
            if (site == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Web site not found.");

            var app = site.Applications["/" + appName];
            var appPath = app == null ? Path.Combine(site.Applications["/"].VirtualDirectories["/"].PhysicalPath, appName) : app.VirtualDirectories["/"].PhysicalPath;
            appPath = Environment.ExpandEnvironmentVariables(appPath);

            webAppInfo.PhysicalPath = appPath;
            webAppInfo.Exist = app != null;

            if(app == null)
            {
                webAppInfo.Links.Add(new Link { Href = ApiUrls.Iis.WebApp(Url, siteName, appName), Method = "POST", Rel = ApiRels.WebAppTemplate });
                webAppInfo.Links.Add(new Link { Href = ApiUrls.Sync.DirectoryTemplate(Url), Method = "POST", Rel = ApiRels.DirTemplate });
            }
            else
            {
                var link = string.Format(ApiUrls.Sync.DirectoryTemplate(Url), appPath);
                webAppInfo.Links.Add(new Link { Href = link, Method = "PUT", Rel = ApiRels.Directory });
            }
            return Request.CreateResponse(HttpStatusCode.Found, webAppInfo);
        }
    }
}