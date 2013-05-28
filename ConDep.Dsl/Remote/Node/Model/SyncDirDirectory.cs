using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConDep.Dsl.Remote.Node.Model
{
    public class SyncDirDirectory
    {
        private readonly List<SyncDirFile> _files = new List<SyncDirFile>();
        private readonly List<SyncDirDirectory> _dirs = new List<SyncDirDirectory>();
        private readonly List<Link> _links = new List<Link>();

        public string FullPath { get; set; }
        public string RelativePath { get; set; }
        public string Attributes { get; set; }
        public IEnumerable<SyncDirFile> Files { get { return _files; } }
        public IEnumerable<SyncDirDirectory> Directories { get { return _dirs; } }
        public List<Link> Links { get { return _links; } }

        public bool EqualTo(DirectoryInfo dirInfo, string dirInfoRootPath)
        {
            if(dirInfo == null) return false;
            if (RelativePath != dirInfo.TrimPath(dirInfoRootPath)) return false;
            if (Attributes != dirInfo.Attributes.ToString()) return false;
            return true;
        }

        public SyncDirDiff Diff(DirectoryInfo directory)
        {
            var srcPaths = directory.GetAllRelativeFilePaths().ToList();
            var dstPaths = GetAllRelativeFilePaths().ToList();

            var syncDirDiff = new SyncDirDiff();

            syncDirDiff.MissingPaths = GetMissingPaths(dstPaths, srcPaths);
            syncDirDiff.ChangedPaths = GetChangedPaths(srcPaths, syncDirDiff.MissingPaths, directory.FullName);
            syncDirDiff.DeletedPaths = GetUnwantedPaths(dstPaths, srcPaths);

            return syncDirDiff;
        }

        private SyncDirFile GetFileByRelativePath(string relativePath)
        {
            foreach (var file in Files.Where(file => file.RelativePath == relativePath))
            {
                return file;
            }

            return Directories.Select(dir => dir.GetFileByRelativePath(relativePath)).FirstOrDefault(file => file != null);
        }

        private SyncDirDirectory GetDirByRelativePath(string relativePath)
        {
            foreach (var dir in Directories.Where(dir => dir.RelativePath == relativePath))
            {
                return dir;
            }

            return Directories.Select(dir => dir.GetDirByRelativePath(relativePath)).FirstOrDefault(dir => dir != null);
        }

        private IEnumerable<SyncPath> GetAllRelativeFilePaths()
        {
            return GetAllRelativeFilePaths(this, FullPath, true);
        }

        private static IEnumerable<SyncPath> GetAllRelativeFilePaths(SyncDirDirectory syncDir, string rootPath, bool isRoot)
        {
            var paths = new List<SyncPath>();
            
            if(!isRoot)
            {
                var path = new SyncPath(syncDir.FullPath, rootPath, syncDir.Attributes) {IsDirectory = true};
                paths.Add(path);  
            }

            paths.AddRange(syncDir.Files.Select(file => new SyncPath(file.FullPath, rootPath, file.Attributes)));
            paths.AddRange(syncDir.Directories.SelectMany(dir => GetAllRelativeFilePaths(dir, rootPath, false)));
            return paths;
        }

        private IEnumerable<SyncPath> GetMissingPaths(IEnumerable<SyncPath> dstPaths, IEnumerable<SyncPath> srcPaths)
        {
            return srcPaths.Except(dstPaths, new SyncPathRelativePathComparer()); //Elements not in dst
        }

        private IEnumerable<SyncPath> GetUnwantedPaths(IEnumerable<SyncPath> dstPaths, IEnumerable<SyncPath> srcPaths)
        {
            return dstPaths.Except(srcPaths, new SyncPathRelativePathComparer()); //Elements not in src
        }

        private IEnumerable<SyncPath> GetChangedPaths(IEnumerable<SyncPath> srcPaths, IEnumerable<SyncPath> missingDirPaths, string rootPath)
        {
            var equalPaths = srcPaths.Except(missingDirPaths, new SyncPathRelativePathComparer()).ToList();
            var nonEqualFilePaths = new List<SyncPath>();

            var equalFiles = equalPaths.Where(x => !x.IsDirectory);
            var equalDirs = equalPaths.Where(x => x.IsDirectory);

            foreach (var file in equalFiles)
            {
                var fileInfo = new FileInfo(Path.Combine(rootPath, file.RelativePath));
                var syncFile = GetFileByRelativePath(file.RelativePath);

                if (!syncFile.EqualTo(fileInfo, rootPath))
                {
                    nonEqualFilePaths.Add(new SyncPath(fileInfo.FullName, rootPath, fileInfo.Attributes.ToString()));        
                }
            }

            foreach(var dir in equalDirs)
            {
                var dirInfo = new DirectoryInfo(Path.Combine(rootPath, dir.RelativePath));
                var syncDir = GetDirByRelativePath(dir.RelativePath);

                if (!syncDir.EqualTo(dirInfo, rootPath))
                {
                    nonEqualFilePaths.Add(new SyncPath(dirInfo.FullName, rootPath, dirInfo.ToString(), true));
                }
            }
            return nonEqualFilePaths;
        } 
    }
}