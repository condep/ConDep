using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ConDep.Dsl.Logging;
using Raven.Client;

namespace ConDep.Server.Api.Controllers
{
    public class ConDepServerLogger : LoggerBase
    {
        private readonly string _execId;
        private ExecutionLog _activeLog;

        public ConDepServerLogger(string execId)
        {
            _execId = execId;
        }

        public override async void Log(string message, Exception ex, TraceLevel traceLevel, params object[] formatArgs)
        {
            using (var session = ConDepServer.DocumentStore.OpenAsyncSession())
            {
                var formattedMessage = (formatArgs != null && formatArgs.Length > 0) ? string.Format(message, formatArgs) : message;
                if (ex != null)
                {
                    formattedMessage += "\nException: " + ex;
                }

                var lines = formattedMessage.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var inlineMessage in lines)
                {
                    var messageToStore = inlineMessage;
                    await GetLog(session, _execId).ContinueWith(x => x.Result.Log(messageToStore, traceLevel));
                }
                await session.SaveChangesAsync();
            }
        }

        public override async void LogSectionStart(string name)
        {
            using (var session = ConDepServer.DocumentStore.OpenAsyncSession())
            {
                await GetLog(session, _execId).ContinueWith(x => x.Result.StartSubLog(name));
                await session.SaveChangesAsync();
            }
        }

        public override async void LogSectionEnd(string name)
        {
            using (var session = ConDepServer.DocumentStore.OpenAsyncSession())
            {
                await GetLog(session, _execId).ContinueWith(x => x.Result.EndSubLog(name));
                await session.SaveChangesAsync();
            }
        }

        public override TraceLevel TraceLevel { get; set; }

        private async Task<ExecutionLog> GetLog(IAsyncDocumentSession session, string execId)
        {
            return await session.LoadAsync<ExecutionLog>("log/" + execId);
        }
    }
}