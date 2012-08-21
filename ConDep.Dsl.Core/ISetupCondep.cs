using System;
using System.Diagnostics;

namespace ConDep.Dsl.Core
{
    public interface ISetupCondep
    {
        bool IsValid(Notification notification);
        WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> onMessage, EventHandler<WebDeployMessageEventArgs> onErrorMessage, WebDeploymentStatus status);
    }
}