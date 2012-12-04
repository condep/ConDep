using System;
using System.IO;
using System.Reflection;

namespace ConDep.Dsl.Resources
{
    public class ConDepResourceFiles
    {
        public static string GetFilePath(string resourceNamespace, string resourceName)
        {
            //Todo: not thread safe
            var tempFolder = Path.GetTempPath();
            var filePath = Path.Combine(tempFolder, resourceName + ".condep");

            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceNamespace + "." + resourceName))
                {
                    using (var writeStream = File.Create(filePath))
                    {
                        stream.CopyTo(writeStream);
                    }
                }
                return filePath;
            }
            catch(Exception ex)
            {
                throw new ConDepResourceNotFoundException(string.Format("Resource [{0}]", resourceName), ex);
            }
        }
    }
}