using System.Web.Http;
using ConDep.Server.Model.DeploymentAggregate;

namespace ConDep.Server.Api.Controllers
{
    public class StatusController : RavenDbController
    {
        public Deployment Get(string id)
        {
            return Session.Load<Deployment>(RavenDb.GetFullId<Deployment>(id));
        }    
    }
}