using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Timers;

namespace ConDep.Server.Infrastructure
{
    public class InMemoryCommandQueue
    {
        private readonly ConcurrentQueue<dynamic> _commandQueue = new ConcurrentQueue<dynamic>();
        private Timer _processQueueScheduler;
        private bool _processing;
        private readonly object _locker = new object();

        public InMemoryCommandQueue()
        {
            Schedule();
        }

        private void Schedule()
        {
            _processQueueScheduler = new Timer(1);
            _processQueueScheduler.Elapsed += OnSchedule;
            _processQueueScheduler.AutoReset = false;
        }

        private void OnSchedule(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (_locker)
                {
                    if (_processing || _commandQueue.Count == 0) return;
                    _processing = true;
                }

                Trace.WriteLine(DateTime.Now.ToLongTimeString() + " - Queue: " + _commandQueue.Count);

                dynamic handlerAndCommand;
                if (_commandQueue.TryDequeue(out handlerAndCommand))
                {
                    var action = handlerAndCommand.Action;
                    var handler = handlerAndCommand.Handler;
                    var command = handlerAndCommand.Command;
                    action(handler, command);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                lock (_locker)
                {
                    _processing = false;
                }
                if (_commandQueue.Count > 0) 
                    _processQueueScheduler.Start();
            }
        }

        public void Enqueue<TCommand>(Action<IHandleCommand<TCommand>, TCommand> action, IHandleCommand<TCommand> handler, TCommand command) where TCommand : ICommand
        {
            var tmp = new HandlerAction<TCommand> {Action = action, Handler = handler, Command = command};
            _commandQueue.Enqueue(tmp);

            lock (_locker)
            {
                if (!_processQueueScheduler.Enabled && !_processing)
                {
                    _processQueueScheduler.Start();
                }
            }
        }

        private class HandlerAction<TCommand>
        {
            public Action<IHandleCommand<TCommand>, TCommand> Action { get; set; }
            public IHandleCommand<TCommand> Handler { get; set; }
            public TCommand Command { get; set; }
        }
    }
}