using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConDep.Dsl.Resources
{
    public class ConDepResourceFiles
    {
        public static string GetFilePath(Assembly assembly, string resourceNamespace, string resourceName, bool keepOriginalFileName = false)
        {
            //Todo: not thread safe
            var tempFolder = Path.GetTempPath();
            var filePath = Path.Combine(tempFolder, resourceName + (keepOriginalFileName ? "" : ".condep"));

            try
            {
                using (var stream = assembly.GetManifestResourceStream(resourceNamespace + "." + resourceName))
                {
                    if(stream == null)
                    {
                        throw new ConDepResourceNotFoundException(string.Format("Unable to find resource [{0}]", resourceName));    
                    }

                    using (var writeStream = File.Create(filePath))
                    {
                        stream.CopyTo(writeStream);
                    }
                }
                return filePath;
            }
            catch (Exception ex)
            {
                throw new ConDepResourceNotFoundException(string.Format("Resource [{0}]", resourceName), ex);
            }
        }

        internal static string GetFilePath(string resourceNamespace, string resourceName, bool keepOriginalFileName = false)
        {
            return GetFilePath(Assembly.GetExecutingAssembly(), resourceNamespace, resourceName, keepOriginalFileName);
        }

        internal static string GetFilePath(string fullResourceName, bool keepOriginalFileName = false)
        {
            var regex = new Regex(@".+\.(.+\.(ps1|psm1))");
            var resourceName = regex.Match(fullResourceName).Groups[1].Value;
            var resourceNamespace = fullResourceName.Replace("." + resourceName, "");
            return GetFilePath(Assembly.GetExecutingAssembly(), resourceNamespace, resourceName, keepOriginalFileName);
        }

        public static string GetFilePath(Assembly assembly, string fullResourceName, bool keepOriginalFileName = false)
        {
            var regex = new Regex(@".+\.(.+\.(ps1|psm1))");
            var match = regex.Match(fullResourceName);
            if(match.Success)
            {
                var resourceName = match.Groups[1].Value;
                if(!string.IsNullOrWhiteSpace(resourceName))
                {
                    var resourceNamespace = fullResourceName.Replace("." + resourceName, "");
                    return GetFilePath(assembly, resourceNamespace, resourceName, keepOriginalFileName);
                }
            }
            return null;
        }
    }
}