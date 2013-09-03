using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ConDep.Dsl.Remote.Node
{
    public class MultipartSyncDirContent : MultipartContent
    {
        public MultipartSyncDirContent() : base("dir-sync-data")
        {
        }

        public MultipartSyncDirContent(string boundary) : base("dir-sync-data", boundary)
        {
        }

        public void Add(HttpContent content, string name, string fileName)
        {
            if (content.Headers.ContentDisposition == null)
            {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("dir-sync-data")
                                                         {
                                                             Name = name,
                                                             FileName = fileName,
                                                             FileNameStar = fileName
                                                         };
            }

            base.Add(content);
        }

        public void Add(HttpContent content, string name, string fileName, FileAttributes attributes, DateTime lastWriteTimeUtc)
        {
            if (content.Headers.ContentDisposition == null)
            {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("dir-sync-data")
                                                         {
                                                             Name = name,
                                                             FileName = fileName,
                                                             FileNameStar = fileName,
                                                         };
                var parameters = content.Headers.ContentDisposition.Parameters;

                parameters.Add(new NameValueHeaderValue("fileAttributes", HttpUtility.UrlEncode(attributes.ToString())));
                parameters.Add(new NameValueHeaderValue("lastWriteTimeUtc", lastWriteTimeUtc.ToFileTime().ToString(CultureInfo.InvariantCulture)));
            }

            base.Add(content);
        }
    }
}