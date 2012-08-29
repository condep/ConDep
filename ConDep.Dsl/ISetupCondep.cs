using System;
using System.Diagnostics;

namespace ConDep.Dsl.Core
{
    public interface ISetupConDep
    {
        bool IsValid(Notification notification);
        WebDeploymentStatus Execute(ConDepOptions options, EventHandler<WebDeployMessageEventArgs> onMessage, EventHandler<WebDeployMessageEventArgs> onErrorMessage, WebDeploymentStatus status);
        WebDeploymentStatus ExecuteContext(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> onMessage, EventHandler<WebDeployMessageEventArgs> onErrorMessage, WebDeploymentStatus status);
    }
}