using System;

namespace ConDep.Client
{
    public class SyncDirFile
    {
        public string Path { get; set; }
        public DateTime LastWriteTimeUtc { get; set; }
        public long Size { get; set; }
        public string Attributes { get; set; }
        public JsonLink Link { get; set; }
    }

    public class JsonLink 
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}