using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using ConDep.Node.Model;

namespace ConDep.Node
{
    public class MultipartSyncDirStreamProvider : MultipartFileStreamProvider
    {
        private int _contentCounter = -1;
        private int _postProcContentMarker = -1;
        private readonly SyncResult _syncResult = new SyncResult();

        public MultipartSyncDirStreamProvider(string rootPath) : base(rootPath)
        {
        }

        public MultipartSyncDirStreamProvider(string rootPath, int bufferSize) : base(rootPath, bufferSize)
        {
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            _contentCounter++;

            if (headers.ContentType != null && headers.ContentType.MediaType.Contains("json"))
            {
                _postProcContentMarker = _contentCounter;
                return new MemoryStream();
            }

            string localFilePath;
            try
            {
                var filename = GetLocalFileName(headers);
                localFilePath = Path.Combine(RootPath, filename);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Something went wrong when getting local file path...", e);
            }

            var fileData = new MultipartFileData(headers, localFilePath);
            FileData.Add(fileData);

            var file = new FileInfo(localFilePath);
            if(file.Exists)
            {
                SyncResult.UpdatedFiles.Add(localFilePath);
                SyncResult.Log.Add("Updated: " + localFilePath);
            }
            else
            {
                Directory.CreateDirectory(file.DirectoryName);
                SyncResult.CreatedFiles.Add(localFilePath);
                SyncResult.Log.Add("Created: " + localFilePath);
            }

            return File.Create(localFilePath, BufferSize, FileOptions.Asynchronous);
        }

        public override Task ExecutePostProcessingAsync()
        {
            foreach (var fileData in FileData)
            {
                var contentDisposition = fileData.Headers.ContentDisposition;

                var filePath = Path.Combine(RootPath, contentDisposition.FileName.Trim('"'));
                
                var fileTimeString = contentDisposition.Parameters.FirstOrDefault(x => x.Name == "lastWriteTimeUtc");
                var fileTimeLong = Convert.ToInt64(fileTimeString.Value);
                var lastWriteTimeUtc = DateTime.FromFileTime(fileTimeLong);
                
                var attributes = contentDisposition.Parameters.FirstOrDefault(x => x.Name == "fileAttributes");

                FileAttributes fileAttributes;
                FileAttributes.TryParse(HttpUtility.UrlDecode(attributes.Value), true, out fileAttributes);

                UpdateFileWithOriginalSettings(filePath, lastWriteTimeUtc, fileAttributes);
            }

            if(_postProcContentMarker > -1)
            {
                var deleteJson = Contents[_postProcContentMarker];
                var data = deleteJson.ReadAsAsync<SyncPostProcContent>().Result;

                if(data != null)
                {
                    DeleteFiles(data);
                    ChangeAttributesOnFolders(data);
                }
            }

            return base.ExecutePostProcessingAsync();
        }

        private void ChangeAttributesOnFolders(SyncPostProcContent data)
        {
            foreach(var dir in data.ChangedDirectories)
            {
                var dirInfo = new DirectoryInfo(dir.Path);
                FileAttributes attributes; 
                if(FileAttributes.TryParse(dir.Attributes, true, out attributes))
                {
                    dirInfo.Attributes = attributes;
                }
                else
                {
                    throw new InvalidAttributesException(string.Format("Unable to parse attributes [{0}].", dir.Attributes));
                }
            }
        }

        private void DeleteFiles(SyncPostProcContent content)
        {
            foreach(var file in content.DeletedFiles)
            {
                var fileInfo = new FileInfo(file.Path) {Attributes = FileAttributes.Normal};
                fileInfo.Delete();
                SyncResult.DeletedFiles.Add(file.Path);
                SyncResult.Log.Add("Deleted: " + file.Path);
            }

            foreach (var dir in content.DeletedDirectories.OrderByDescending(x => x.RelativePath))
            {
                var dirInfo = new DirectoryInfo(dir.Path) { Attributes = FileAttributes.Normal };
                dirInfo.Delete();
                SyncResult.DeletedDirectories.Add(dir.Path);
                SyncResult.Log.Add("Deleted: " + dir.Path);
            }
        }

        private void UpdateFileWithOriginalSettings(string filePath, DateTime lastWriteTimeUtc, FileAttributes fileAttributes)
        {
            File.SetLastWriteTime(filePath, lastWriteTimeUtc);
            File.SetAttributes(filePath, fileAttributes);
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Trim('"');// Path.GetFileName();
        }

        public SyncResult SyncResult
        {
            get { return _syncResult; }
        }
    }
}