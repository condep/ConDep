using System;
using System.ServiceProcess;

namespace ConDep.WebQ.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var timeout = 20;
            if(args.Length > 0)
            {
                timeout = Convert.ToInt32(args[0]);
            }
            ServiceBase.Run(new ServiceBase[] {new ConDepWebQService(timeout)});
        }
    }
}
