using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using ConDep.Server.Api.Model;

namespace ConDep.Server.Api.Controllers
{
    public class ModulesController : ApiController
    {
        public IEnumerable<ModuleInfo> Get()
        {
            var currDir = Path.GetDirectoryName(GetType().Assembly.Location);
            var modulesDir = Path.Combine(currDir, "modules");

            return Directory.GetFiles(modulesDir)
                .Where(x => 
                    x.ToLower().EndsWith(".dll") || 
                    x.ToLower().EndsWith(".csx"))
                .Select(file => new FileInfo(file)).Select(fileInfo => new ModuleInfo()
                {
                    Name = Path.GetFileNameWithoutExtension(fileInfo.FullName),
                    FileName = fileInfo.Name,
                    FullFileName = fileInfo.FullName,
                    CreatedUtc = fileInfo.CreationTimeUtc,
                    Directory = fileInfo.Directory.FullName,
                    LastUploadedUtc = fileInfo.LastWriteTimeUtc,
                    Type = GetModuleType(Path.GetExtension(fileInfo.Name.ToLower()))
                });
        }

        private string GetModuleType(string extension)
        {
            switch (extension)
            {
                case ".dll": return "Assembly";
                case ".csx": return "ScriptCS";
                default: return "Unknown";
            }
        }
    }
}