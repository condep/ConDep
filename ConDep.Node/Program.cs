using System.ServiceProcess;

namespace ConDep.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:80/ConDepNode/";
            if(args.Length > 0)
            {
                url = args[0];
            }

            ServiceBase.Run(new ServiceBase[] { new NodeService(url) });
        }
    }
}