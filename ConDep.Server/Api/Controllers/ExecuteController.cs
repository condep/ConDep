using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using ConDep.Dsl;
using ConDep.Dsl.Remote.Node.Model;

namespace ConDep.Server.Api.Controllers
{
    public class ExecuteController : ApiController
    {
         public List<Link> Post(string module, string artifact, string env)
         {
             /**
                1) Load assembly
                2) Create instance of artifact
                3) Execute on new thread
                4) Return execution id, link to log and link to status
             **/
             var log = new ExecutionLog(Guid.NewGuid().ToString());
             using (var session = ConDepServer.DocumentStore.OpenSession())
             {
                 session.Store(log);
                 session.SaveChanges();
             }

             var executor = new ConDepExecutor(ConDepServer.DocumentStore, log.ExecId, module, artifact, env);
             var thread = new Thread(executor.Execute);
             thread.Start();

             return new List<Link>
                 {
                     new Link()
                         {
                             Href = string.Format("/condepserver/api/log/{0}", log.ExecId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/log"
                         },
                    new Link()
                         {
                             Href = string.Format("/condepserver/api/status/{0}", log.ExecId),
                             Method = "GET",
                             Rel = "http://www.con-dep.net/rels/server/status"
                         }
                 };
         }
    }
}