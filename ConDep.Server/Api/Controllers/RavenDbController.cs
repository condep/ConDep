using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;

namespace ConDep.Server.Api.Controllers
{
    public class RavenDbController : ApiController
    {
        public IDocumentSession Session { get; set; }

        public override Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Web.Http.Controllers.HttpControllerContext controllerContext, System.Threading.CancellationToken cancellationToken)
        {
            Session = RavenDb.DocumentStore.OpenSession();
            return base.ExecuteAsync(controllerContext, cancellationToken)
                       .ContinueWith(task =>
                           {
                               using (Session)
                               {
                                   if (task.Status != TaskStatus.Faulted && Session != null)
                                   {
                                       Session.SaveChanges();
                                   }
                               }
                               return task;
                           }).Unwrap();
        }
    }
}