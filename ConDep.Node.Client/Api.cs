using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using ConDep.Node.Client.Model;
using Newtonsoft.Json.Linq;

namespace ConDep.Node.Client
{
    public class Api
    {
        private HttpClient _client;

        public Api(string url)
        {
            _client = new HttpClient { BaseAddress = new Uri(url) };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public SyncResult SyncDir(string srcPath, string dstPath)
        {
            var availableApiResourcesResponse = _client.GetAsync("api").Result;
            var availableApiResourcesContent = availableApiResourcesResponse.Content.ReadAsAsync<JToken>().Result;

            var url = (from link in availableApiResourcesContent
                       where link.Value<string>("rel") == "http://www.con-dep.net/rels/sync/dir_template"
                       select link.Value<string>("href")).SingleOrDefault();

            var syncResponse = _client.GetAsync(string.Format(url, dstPath)).Result;

            if (syncResponse.IsSuccessStatusCode)
            {
                var nodeDir = syncResponse.Content.ReadAsAsync<SyncDirDirectory>().Result;
                return CopyFiles(srcPath, _client, nodeDir);
            }
            return null;
        }

        public SyncResult SyncFile(string srcPath, string dstPath)
        {
            var availableApiResourcesResponse = _client.GetAsync("api").Result;
            var availableApiResourcesContent = availableApiResourcesResponse.Content.ReadAsAsync<JToken>().Result;

            var url = (from link in availableApiResourcesContent
                       where link.Value<string>("rel") == "http://www.con-dep.net/rels/sync/file_template"
                       select link.Value<string>("href")).SingleOrDefault();

            var syncResponse = _client.GetAsync(string.Format(url, dstPath)).Result;

            if (syncResponse.IsSuccessStatusCode)
            {
                var nodeFile = syncResponse.Content.ReadAsAsync<SyncDirFile>().Result;
                return CopyFile(srcPath, _client, nodeFile);
            }
            return null;
        }

        private SyncResult CopyFile(string srcFile, HttpClient client, SyncDirFile nodeFile)
        {
            var message = new HttpRequestMessage();

            var clientFile = new FileInfo(srcFile);
            if(nodeFile.EqualTo(clientFile, clientFile.Directory.FullName))
            {
                return new SyncResult();
            }

            var fileStream = new FileStream(clientFile.FullName, FileMode.Open);
            var content = new StreamContent(fileStream);

            var link = nodeFile.Links.GetByRel("http://www.con-dep.net/rels/sync/file");
            var url = string.Format(link.Href, clientFile.LastWriteTimeUtc.ToFileTime(), clientFile.Attributes.ToString());

            message.Method = link.HttpMethod;
            message.Content = content;
            message.RequestUri = new Uri(url);

            var result = client.SendAsync(message).ContinueWith(task =>
                                                                    {
                                                                        if (task.Result.IsSuccessStatusCode)
                                                                        {
                                                                            var syncResult = task.Result.Content.ReadAsAsync<SyncResult>().Result;
                                                                            return syncResult;
                                                                        }
                                                                        return null;
                                                                    });
            result.Wait();
            return result.Result;
        }

        private SyncResult CopyFiles(string srcRoot, HttpClient client, SyncDirDirectory nodeDir)
        {
            var message = new HttpRequestMessage();
            var content = new MultipartSyncDirContent();

            var clientDir = new DirectoryInfo(srcRoot);
            var diffs = nodeDir.Diff(clientDir);

            var files = diffs.MissingAndChangedPaths;

            foreach (var file in files)
            {
                if (file.IsDirectory) continue;

                var fileInfo = new FileInfo(file.Path);
                var fileStream = new FileStream(fileInfo.FullName, FileMode.Open);
                content.Add(new StreamContent(fileStream), "file", file.RelativePath, fileInfo.Attributes, fileInfo.LastWriteTimeUtc);
            }

            var postProcContent = new SyncPostProcContent
            {
                DeletedFiles = diffs.DeletedFiles,
                DeletedDirectories = diffs.DeletedDirectories,
                ChangedDirectories = diffs.ChangedDirectories
            };
            content.Add(new ObjectContent<SyncPostProcContent>(postProcContent, new JsonMediaTypeFormatter()));

            var link = nodeDir.Links.GetByRel("http://www.con-dep.net/rels/sync/directory");

            message.Method = link.HttpMethod;
            message.Content = content;
            message.RequestUri = new Uri(link.Href);

            var result = client.SendAsync(message).ContinueWith(task =>
            {
                if (task.Result.IsSuccessStatusCode)
                {
                    var syncResult = task.Result.Content.ReadAsAsync<SyncResult>().Result;
                    return syncResult;
                }
                return null;
            });
            result.Wait();
            return result.Result;
        }

    }
}
