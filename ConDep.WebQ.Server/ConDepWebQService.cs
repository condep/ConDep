using System.ServiceProcess;

namespace ConDep.WebQ.Server
{
    partial class ConDepWebQService : ServiceBase
    {
        private readonly int _timeout;
        private ConDepWebServer _webServer;

        public ConDepWebQService(int timeout)
        {
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

            _webServer = new ConDepWebServer(_timeout, EventLog);
            //var timer = new Timer(5000);
            //timer.Elapsed += _webServer.OnQueueCleanup;
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
