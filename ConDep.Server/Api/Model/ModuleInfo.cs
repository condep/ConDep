using System;

namespace ConDep.Server.Api.Model
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public DateTime LastUploadedUtc { get; set; }
        public string FileName { get; set; }
        public string FullFileName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Directory { get; set; }
        public string Type { get; set; }
    }
}