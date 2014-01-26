using System;
using System.Timers;
using ConDep.Server.Commands;
using ConDep.Server.Infrastructure;

namespace ConDep.Server.Application
{
    public class DeploymentScheduler : IDisposable
    {
        private readonly ICommandBus _bus;
        private bool _disposed;
        private Timer _processTimer;
        private Timer _cleanupTimer;

        public DeploymentScheduler(ICommandBus bus)
        {
            _bus = bus;
        }

        public void ScheduleQueueCleanup(int cleanupIntervalInSeconds)
        {
            _cleanupTimer = new Timer(1000 * cleanupIntervalInSeconds);
            _cleanupTimer.Elapsed += CleanupDeploymentQueues;
            _cleanupTimer.Start();
        }

        public void ScheduleQueueProcessing(int processIntervalInSeconds)
        {
            _processTimer = new Timer(1000 * processIntervalInSeconds);
            _processTimer.Elapsed += ProcessDeploymentQueues;
            _processTimer.Start();
        }

        private void ProcessDeploymentQueues(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var cmd = new ProcessDeploymentQueue();
            _bus.Send(cmd);
        }

        private void CleanupDeploymentQueues(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var cmd = new CleanupDeploymentQueue(new TimeSpan(0, 30, 0));
            _bus.Send(cmd);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (!disposing) return;

            if (_processTimer != null)
            {
                _processTimer.Elapsed -= ProcessDeploymentQueues;
                _processTimer.Dispose();
            }

            if (_cleanupTimer != null)
            {
                _cleanupTimer.Elapsed -= CleanupDeploymentQueues;
                _cleanupTimer.Dispose();
            }

            _disposed = true;
        }

        ~DeploymentScheduler()
        {
            Dispose(false);
        }

    }
}