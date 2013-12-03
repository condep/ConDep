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
                        Status = "Added to execution queue",
                        UpdatedUtc = DateTime.UtcNow
                    };
                session.Store(executionStatus);

                session.SaveChanges();
            }

            return new List<Link>
                 {
                    new Link()
                        {
                            Href = string.Format("/condepserver/api/queue"),
                            Method = "GET",
                            Rel = "http://www.con-dep.net/rels/server/queue"
                        },
                    new Link()
                         {
                             Href = string.Format("/condepserver/api/queue/{0}", execId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/queue"
                         },
                     new Link()
                         {
                             Href = string.Format("/condepserver/api/log/{0}", execId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/log"
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