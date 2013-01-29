using System.ServiceProcess;

namespace ConDep.WebQ.Server
{
    partial class ConDepWebQService : ServiceBase
    {
        private readonly int _port;
        private readonly int _timeout;
        private ConDepWebServer _webServer;

        public ConDepWebQService(int timeout, int port)
        {
            _port = port;
            _timeout = timeout;

            InitializeComponent();

            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;
            this.EventLog.EnableRaisingEvents = true;
            this.EventLog.Source = "ConDepWebQService";
            this.EventLog.Log = "Application";

        }

        protected override void OnStart(string[] args)
        {
            if(_webServer != null)
            {
                _webServer.Dispose();
            }

            _webServer = new ConDepWebServer(_port, _timeout, EventLog);
            _webServer.Start();
        }

        protected override void OnStop()
        {
            if(_webServer != null)
            {
                _webServer.Stop();
            }
        }
    }
}
