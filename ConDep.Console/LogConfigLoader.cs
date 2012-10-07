using System;
using System.IO;
using log4net.Config;

namespace ConDep.Console
{
    internal class LogConfigLoader
    {
        public void Load()
        {
            var logpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "condep.log4net.xml");
            if(File.Exists(logpath))
            {
                XmlConfigurator.Configure(new FileInfo(logpath));
            }
            else
            {
                var type = GetType();

                using(var logConfigStream = type.Module.Assembly.GetManifestResourceStream(type.Namespace + ".internal.condep.log4net.xml"))
                {
                    XmlConfigurator.Configure(logConfigStream);
                }
            }
        }
    }
}