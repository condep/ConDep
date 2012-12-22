using System;
using System.IO;
using System.Reflection;

namespace ConDep.Dsl.Resources
{
    public class ConDepResourceFiles
    {
        public static string GetFilePath(Assembly assembly, string resourceNamespace, string resourceName)
        {
            //Todo: not thread safe
            var tempFolder = Path.GetTempPath();
            var filePath = Path.Combine(tempFolder, resourceName + ".condep");

            try
            {
                using (var stream = assembly.GetManifestResourceStream(resourceNamespace + "." + resourceName))
                {
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

        public static string GetFilePathInternal(string resourceNamespace, string resourceName)
        {
            return GetFilePath(Assembly.GetExecutingAssembly(), resourceNamespace, resourceName);
        }
    }
}