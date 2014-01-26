using System;
using ConDep.Dsl.SemanticModel;
using ConDep.Server.Model.DeploymentAggregate;
using Raven.Client;

namespace ConDep.Server
{
    public class ExecutionInfoHandler
    {
        public void UpdateResult(string id, ConDepExecutionResult result)
        {
            //using (var session = RavenDb.DocumentStore.OpenSession())
            //{
            //    var executionInfo = session.Load<ExecutionInfo>(RavenDb.GetFullId<ExecutionInfo>(id));
            //    executionInfo.FinishedUtc = DateTime.UtcNow;
            //    if (result.HasExceptions())
            //    {
            //        result.ExceptionMessages.ForEach(
            //            message =>
            //            executionInfo.Exceptions.Add(new ExecutionMessage
            //            {
            //                DateUtc = message.DateTime,
            //                Message = message.Exception.Message
            //            }));
            //    }

            //    if (result.Cancelled)
            //    {
            //        executionInfo.Status = ExecutionStatus.Cancelled;
            //    }
            //    else
            //    {
            //        executionInfo.Status = result.Success ? ExecutionStatus.Success : ExecutionStatus.Failed;
            //    }
            //    session.SaveChanges();
            //}
        }

        public void AddEvent(string id, DeploymentMessage deploymentMessage)
        {
            //using (var session = RavenDb.DocumentStore.OpenSession())
            //{
            //    var executionInfo = session.Load<ExecutionInfo>(RavenDb.GetFullId<ExecutionInfo>(id));
            //    executionInfo.Events.Add(executionMessage);
            //    executionInfo.Status = ExecutionStatus.InProgress;
            //}
        }
    }
}