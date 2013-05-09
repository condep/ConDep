using System.IO;
using ConDep.Node.Model;

namespace ConDep.Node
{
    public class SyncDirHandler
    {
        private string MakePathRelative(string rootPath, string path)
        {
            if (!rootPath.EndsWith("\\"))
                rootPath += "\\";

            return path.Replace(rootPath, "");
        }

        public SyncDirDirectory GetSubDirInfo(string rootPath, string dirUrl, string fileUrl, DirectoryInfo dirInfo)
        {
            var dir = new SyncDirDirectory() { FullPath = dirInfo.FullName, RelativePath = MakePathRelative(rootPath, dirInfo.FullName), Attributes = dirInfo.Attributes.ToString() };
            var link = string.Format(dirUrl, dirInfo.FullName);

            if (!dirInfo.Exists)
            {
                dir.Links.Add(new Link() { Rel = "http://www.con-dep.net/rels/sync/directory", Method = "POST", Href = string.Format("{0}", link) });
                return dir;
            }

            dir.Links.Add(new Link() { Rel = "self", Href = string.Format("{0}", link), Method = "GET" });
            dir.Links.Add(new Link() { Rel = "http://www.con-dep.net/rels/sync/directory", Method = "PUT", Href = string.Format("{0}", link) });

            foreach (var childDir in dirInfo.EnumerateDirectories())
            {
                dir.Directories.Add(GetSubDirInfo(rootPath, dirUrl, fileUrl, childDir));
            }

            foreach (var childFile in dirInfo.EnumerateFiles())
            {
                //var secDescriptor = childFile.GetAccessControl().GetSecurityDescriptorSddlForm(AccessControlSections.);
                var file = new SyncDirFile()
                               {
                                   FullPath = childFile.FullName,
                                   RelativePath = MakePathRelative(rootPath, childFile.FullName),
                                   Attributes = childFile.Attributes.ToString(),
                                   LastWriteTimeUtc = childFile.LastWriteTimeUtc,
                                   Size = childFile.Length,
                               };
                var fileLink = string.Format(fileUrl, childFile.FullName);
                file.Links.Add(new Link { Href = string.Format("{0}", fileLink), Rel = "self", Method = "GET" });
                file.Links.Add(new Link { Href = string.Format("{0}", fileLink), Rel = "http://www.con-dep.net/rels/sync/file", Method = "PUT" });
                file.Links.Add(new Link { Href = string.Format("{0}", link), Rel = "http://www.con-dep.net/rels/sync/directory", Method = "PUT" });
                dir.Files.Add(file);
            }
            return dir;
        }

        public SyncDirFile GetFileInfo(string rootPath, string fileUrl, string dirUrl, FileInfo fileInfo)
        {
            var link = string.Format(fileUrl, fileInfo.FullName);
            var file = new SyncDirFile()
                           {
                               FullPath = fileInfo.FullName, 
                               RelativePath = fileInfo.Name, 
                               Attributes = fileInfo.Attributes.ToString()
                           };

            if (!fileInfo.Exists)
            {
                file.Links.Add(new Link() { Rel = "http://www.con-dep.net/rels/sync/file", Method = "POST", Href = string.Format("{0}{1}", link, "&lastWriteTimeUtc={0}&fileAttributes={1}") });
                return file;
            }

            var dirLink = string.Format(dirUrl, fileInfo.Directory.FullName);
            file.LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            file.Size = fileInfo.Length;

            file.Links.Add(new Link() { Rel = "self", Href = string.Format("{0}", link), Method = "GET" });
            file.Links.Add(new Link() { Rel = "http://www.con-dep.net/rels/sync/file", Method = "PUT", Href = string.Format("{0}{1}", link, "&lastWriteTimeUtc={0}&fileAttributes={1}") });
            file.Links.Add(new Link() { Rel = "http://www.con-dep.net/rels/sync/directory", Method = "PUT", Href = string.Format("{0}", dirLink) });
            return file;
        }
    }
}