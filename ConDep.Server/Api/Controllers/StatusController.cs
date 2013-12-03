using System.Web.Http;
using ConDep.Server.Api.Model;

namespace ConDep.Server.Api.Controllers
{
    public class StatusController : ApiController
    {
        public ExecutionStatus Get(string id)
        {
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                return session.Load<ExecutionStatus>("execution_status/" + id);
            }
        }    
    }
}