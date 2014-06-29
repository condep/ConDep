using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using ConDep.Server.Api.ViewModel;

namespace ConDep.Server.Api.Controllers.Admin
{
    [Route("api/admin/loadbalancer")]
    public class LoadBalancerController : RavenDbController
    {
        public HttpResponseMessage Get()
        {
            var loadBalancers = Session.Query<LoadBalancer>().ToList();
            return Request.CreateResponse(HttpStatusCode.OK, loadBalancers);
        }

        public HttpResponseMessage Post(LoadBalancer lb)
        {
            Session.Store(lb);
            return Request.CreateResponse(HttpStatusCode.Created, Session.Advanced.GetDocumentId(lb));
        }

        [Route("api/admin/loadbalancer/{id}")]
        public HttpResponseMessage Put(int id, LoadBalancer lb)
        {
            Session.Store(lb);
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        [Route("api/admin/loadbalancer/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var storedLb = Session.Load<LoadBalancer>(id);
            Session.Delete(storedLb);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("api/admin/lookups/schedulingalgorithms")]
        [HttpGet]
        public HttpResponseMessage SchedulingAlgorithms()
        {
            Thread.Sleep(2000);
            return Request.CreateResponse(HttpStatusCode.OK, ConvertEnum<SchedulingAlgorithm>());
        }

        [Route("api/admin/lookups/suspendstrategy")]
        [HttpGet]
        public HttpResponseMessage SuspendStrategy()
        {
            Thread.Sleep(2000);
            return Request.CreateResponse(HttpStatusCode.OK, ConvertEnum<SuspendStrategy>());
        }

        private List<EnumItem> ConvertEnum<T>()
        {
            var values = typeof(T).GetEnumValues().OfType<T>().ToList();

            return values.Select(value => new EnumItem(Convert.ToInt32(value), value.ToString())).ToList();
        }
    }

    public class EnumItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public EnumItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}