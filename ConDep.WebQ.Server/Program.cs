using System;
using System.ServiceProcess;
using System.Linq;

namespace ConDep.WebQ.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var timeout = 30;
            var port = 80;

            if(args.Length > 0)
            {
                var portParam = args.SingleOrDefault(x => x.StartsWith("port") || x.StartsWith("/port") || x.StartsWith("-port") || x.StartsWith("--port"));
                var timeoutParam = args.SingleOrDefault(x => x.StartsWith("timeout") || x.StartsWith("/timeout") || x.StartsWith("-timeout") || x.StartsWith("--timeout"));

                if(!string.IsNullOrWhiteSpace(portParam))
                {
                    var parsedPort = ParseParam(portParam);
                    port = parsedPort.HasValue ? parsedPort.Value : port;
                }

                if(!string.IsNullOrWhiteSpace(timeoutParam))
                {
                    var parsedTimeout = ParseParam(timeoutParam);
                    timeout = parsedTimeout.HasValue ? parsedTimeout.Value : timeout;
                }
            }
            ServiceBase.Run(new ServiceBase[] {new ConDepWebQService(timeout, port)});
        }

        private static int? ParseParam(string param)
        {
            if (param.Contains("="))
            {
                var split = param.Split('=');
                if (split.Length == 2)
                {
                    return Convert.ToInt32(split[1]);
                }
            }
            return null;
        }
    }
}
