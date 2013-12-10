using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ConDep.Dsl.Remote.Node.Model;
using ConDep.Server.Api.Model;

namespace ConDep.Server.Api.Controllers
{
    public class ExecuteController : ApiController
    {
        //http://localhost/condepserver/api/execute?module=ConDepSamples&artifact=DotNetWebSslApplication&env=dev
        public async Task<List<Link>> Post(string module, string artifact, string env)
        {
            var execId = Guid.NewGuid().ToString();

            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                var queueItem = new QueueItem()
                    {
                        ExecId = execId,
                        CreatedUtc = DateTime.UtcNow,
                        QueueStatus = QueueStatus.InQueue,
                        ExecutionData = new QueueExecutionData
                            {
                                Module = module,
                                Artifact = artifact,
                                Environment = env,
                            }
                    };
                session.Store(queueItem);

                var executionStatus = new ExecutionStatus()
                    {
                        ExecId = execId,
                        StartedUtc = DateTime.UtcNow,
                        Environment = env,
                        Module = module,
                        RelativeLogLocation = string.Format("{0}/{1}/{2}", "logs", env, execId + ".log")
                    };
                session.Store(executionStatus);

                session.SaveChanges();
            }

            return new List<Link>
                 {
                    new Link()
                         {
                             Href = string.Format("/condepserver/api/queue/{0}", execId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/queue"
                         },
                     new Link()
                         {
                             Href = string.Format("/condepserver/api/logs/{0}/{1}", env, execId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/logs"
                         },
                    new Link()
                         {
                             Href = string.Format("/condepserver/api/status/{0}", execId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/status"
                         }
                 };
        }
    }
}