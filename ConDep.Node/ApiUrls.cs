using System.Web.Http.Routing;

namespace ConDep.Node
{
    public static class ApiUrls
    {
        public static string Home(UrlHelper url) { return url.Link("Api", new { controller = "home" }); }

        public static class Sync
        {
            public static string Home(UrlHelper url) { return url.Link("Api", new { controller = "sync" }); }
            public static string Directory(UrlHelper url) { return url.Link("Sync", new { controller = "Directory" }); }
            public static string DirectoryTemplate(UrlHelper url) { return url.Link("Sync", new { controller = "Directory" }) + "?path={0}"; }
            public static string FileTemplate(UrlHelper url) { return url.Link("Sync", new { controller = "File" }) + "?path={0}"; }
        }
    }
}