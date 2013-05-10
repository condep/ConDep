using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Node.Model;
using Microsoft.Web.Administration;

namespace ConDep.Node.Controllers.Iis
{
    public class IisController : ApiController
    {
        private readonly PathValidator _pathValidator;

        public IisController() : this(new PathValidator())
        {
        }

        public IisController(PathValidator pathValidator)
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

            if (app == null)
            {
                webAppInfo.Links.Add(new Link { Href = ApiUrls.Iis.WebApp(Url, siteName, appName), Method = "POST", Rel = ApiRels.WebAppTemplate });
                webAppInfo.Links.Add(new Link { Href = ApiUrls.Sync.DirectoryTemplate(Url), Method = "POST", Rel = ApiRels.DirTemplate });
            }
            else
            {
                var link = string.Format(ApiUrls.Sync.DirectoryTemplate(Url), appPath);
                webAppInfo.Links.Add(new Link { Href = link, Method = "PUT", Rel = ApiRels.Directory });
            }
            return Request.CreateResponse(HttpStatusCode.OK, webAppInfo);
        }

        public HttpResponseMessage Post(string siteName, string appName, string path)
        {
            var manager = new ServerManager();
            var site = manager.Sites[siteName];
            if (site == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Web site not found.");

            if (!_pathValidator.ValidPath(path))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                                                                            "Invalid path. Path cannot be root of drive, or any of the common system and windows folders."));
            }

            CreateWebApp(manager, site, appName, path);

            return Request.CreateResponse(HttpStatusCode.Created);

        }

        private void CreateWebApp(ServerManager manager, Site site, string appName, string appPath)
        {
            site.Applications.Add("/" + appName, appPath);
            manager.CommitChanges();
        }

    }
}