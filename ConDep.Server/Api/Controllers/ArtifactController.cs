using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using ConDep.Server.Execution;

namespace ConDep.Server.Api.Controllers
{
    public class ArtifactController : ApiController
    {
        public List<Link> Get()
        {
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dirInfo = new DirectoryInfo(Path.Combine(executingPath, "modules"));

            var returnValues = new List<Link>();

            var appDomain = AppDomain.CreateDomain("ArtifactLookup", null, AppDomain.CurrentDomain.SetupInformation);
            var resolver = (ArtifactResolver)appDomain.CreateInstanceAndUnwrap(typeof(ArtifactResolver).Assembly.FullName, typeof(ArtifactResolver).FullName);

            try
            {
                foreach (var dll in dirInfo.GetFiles("*.dll"))
                {
                    var apps = resolver.Resolve(dll.FullName);

                    returnValues.AddRange(apps.Select(artifact => new Link
                        {
                            Href = string.Format("/condepserver/api/execute?module={0}&artifact={1}&env={{0}}", 
                            Path.GetFileNameWithoutExtension(dll.Name), artifact), 
                            Method = HttpMethod.Post, Rel = ""
                        }));
                }
                return returnValues;
            }
            finally
            {
                AppDomain.Unload(appDomain);
            }
        }
    }
}