namespace ConDep.Node.Model
{
    public class SyncPath
    {
        public string RelativePath { get; set; }
        public string Path { get; set; }
        public string RootPath { get; set; }
        public bool IsDirectory { get; set; }
        public string Attributes { get; set; }
    }
}