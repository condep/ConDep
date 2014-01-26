using System.ServiceProcess;

namespace ConDep.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:80/ConDepServer/";
            if (args.Length > 0)
            {
                url = args[0];
            }

#if(DEBUG)
            var service = new ConDepServer(url);
            service.Start(args);
#else
            ServiceBase.Run(new ServiceBase[] { new ConDepServer(url) });
#endif

        }
    }
}