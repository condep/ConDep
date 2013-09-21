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

            ServiceBase.Run(new ServiceBase[] { new ConDepServer(url) });
        }
    }
}