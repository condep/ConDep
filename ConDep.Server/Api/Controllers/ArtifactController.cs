using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using ConDep.Dsl;
using ConDep.Dsl.Remote.Node.Model;

namespace ConDep.Server.Api.Controllers
{
    public class ArtifactController : ApiController
    {
        public List<Link> Get()
        {
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dirInfo = new DirectoryInfo(Path.Combine(executingPath, "modules"));

            var returnValues = new List<Link>();
            foreach (var dll in dirInfo.GetFiles())
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                var artifacts = assembly.GetTypes().Where(t => typeof(ApplicationArtifact).IsAssignableFrom(t));
                foreach (var artifact in artifacts)
                {
                    var link = new Link
                    {
                        Href = string.Format("/condepserver/api/execute?module={0}&artifact={1}&env={{0}}", assembly.GetName().Name, artifact.FullName),
                        Method = "POST",
                        Rel = ""
                    };
                    returnValues.Add(link);
                }
            }
            return returnValues;
        }
        
    }
}