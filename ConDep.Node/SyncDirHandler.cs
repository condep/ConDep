using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConDep.Client
{
    public class SyncDirHandler
    {
        public SyncDirDirectory GetSubDirInfo(string dirUrl, string fileUrl, DirectoryInfo dirInfo)
        {
            var dir = new SyncDirDirectory() { Path = dirInfo.FullName, Attributes = dirInfo.Attributes.ToString(), Link = new JsonLink() { Rel = "self", Href = string.Format("{0}?path={1}", dirUrl, dirInfo.FullName) } };
            if (!dirInfo.Exists)
            {
                return dir;
            }

            foreach (var childDir in dirInfo.EnumerateDirectories())
            {
                dir.Directories.Add(GetSubDirInfo(dirUrl, fileUrl, childDir));
            }

            foreach (var childFile in dirInfo.EnumerateFiles())
            {
                //var secDescriptor = childFile.GetAccessControl().GetSecurityDescriptorSddlForm(AccessControlSections.);
                var file = new SyncDirFile()
                               {
                                   Path = childFile.FullName,
                                   Attributes = childFile.Attributes.ToString(),
                                   LastWriteTimeUtc = childFile.LastWriteTimeUtc,
                                   Size = childFile.Length,
                                   Link = new JsonLink { Href = string.Format("{0}?path={1}", fileUrl, childFile.FullName), Rel = "self" }
                               };
                dir.Files.Add(file);
            }
            return dir;
        }

        public SyncDirContainer GetDiffs(SyncDirDirectory clientDir, SyncDirDirectory serverDir)
        {
            var clientRoot = clientDir.Path;
            var serverRoot = serverDir.Path;

            var clientFiles = new List<string>();
            EnumerateFilePaths(clientRoot, clientFiles, clientDir);

            var serverFiles = new List<string>();
            EnumerateFilePaths(serverRoot, serverFiles, serverDir);

            var missingOnServer = GetMissingFilesOnServer(clientFiles, serverFiles).ToList();
            var missingOnClient = GetMissingFilesOnClient(clientFiles, serverFiles).ToList();

            var commonOnBoth = serverFiles.Except(missingOnServer);

            var fileDiffs = GetDiffsOnFiles(commonOnBoth, clientDir, serverDir);

            var container = new SyncDirContainer();
            container.FilesToCopy.AddRange(missingOnServer);
            container.FilesToDelete.AddRange(missingOnClient);
            container.FilesToUpdate.AddRange(fileDiffs);
            return container;
        }

        private IEnumerable<string> GetDiffsOnFiles(IEnumerable<string> commonOnBoth, SyncDirDirectory clientDir, SyncDirDirectory serverDir)
        {
            var clientRoot = clientDir.Path;
            var onBoth = commonOnBoth as string[] ?? commonOnBoth.ToArray();

            var listOfDiffs = new List<SyncDirFile>();

            return RecurseDiffFiles(clientDir, serverDir, clientRoot, onBoth).ToList();
        }

        private static IEnumerable<string> RecurseDiffFiles(SyncDirDirectory clientDir, SyncDirDirectory serverDir, string clientRoot,
                                             string[] onBoth)
        {
            foreach (var file in clientDir.Files)
            {
                var relativePath = file.Path.Replace(clientRoot, "");
                if (onBoth.Contains(relativePath))
                {
                    var serverFile = serverDir.GetByRelativePath(relativePath);
                    if (file.LastWriteTimeUtc != serverFile.LastWriteTimeUtc || file.Size != serverFile.Size ||
                        file.Attributes != serverFile.Attributes)
                    {
                        yield return file.Path;
                    }
                }
            }

            foreach (var dir in clientDir.Directories)
            {
                foreach(var file in RecurseDiffFiles(dir, serverDir, clientRoot, onBoth))
                {
                    yield return file;
                }
            }
        }

        private IEnumerable<string> GetMissingFilesOnServer(IEnumerable<string> clientFiles, IEnumerable<string> serverFiles)
        {
            return clientFiles.Where(filePath => !serverFiles.Contains(filePath)).ToList();
        }

        private IEnumerable<string> GetMissingFilesOnClient(IEnumerable<string> clientFiles, IEnumerable<string> serverFiles)
        {
            return serverFiles.Where(filePath => !clientFiles.Contains(filePath)).ToList();
        }

        private void EnumerateFilePaths(string root, List<string> files, SyncDirDirectory dir)
        {
            files.AddRange(dir.Files.Select(file => file.Path.Replace(root, "")));
            foreach (var subdir in dir.Directories)
            {
                EnumerateFilePaths(root, files, subdir);
            }
        }
    }
}