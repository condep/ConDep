using System;
using System.Diagnostics;

namespace ConDep.Dsl
{
    public interface ISetupConDep
    {
        bool IsValid(Notification notification);
        WebDeploymentStatus Execute(ConDepOptions options, EventHandler<WebDeployMessageEventArgs> onMessage, EventHandler<WebDeployMessageEventArgs> onErrorMessage, WebDeploymentStatus status);
        WebDeploymentStatus ExecuteAllContextOperations(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> onMessage, EventHandler<WebDeployMessageEventArgs> onErrorMessage, WebDeploymentStatus status);
        void PrintExecutionSequence(ConDepOptions options, int level);
        //ConDepOptions Options { get; set; }
    }
}