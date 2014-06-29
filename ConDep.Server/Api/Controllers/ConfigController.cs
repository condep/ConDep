using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Server.Domain.Environment.Model;

namespace ConDep.Server.Api.Controllers
{
    public class ConfigController : RavenDbController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                var configs = Session.Advanced.LoadStartingWith<DeploymentEnvironment>(RavenDb.GetFullId<DeploymentEnvironment>("")) ?? new DeploymentEnvironment[0];

                var links = configs.Select(config => this.GetLinkFor<ConfigController>(HttpMethod.Get, config.Id)).ToList();

                return Request.CreateResponse(HttpStatusCode.Found, links);
            }
            catch
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Something bad happend."));
            }
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                var config = Session.Load<DeploymentEnvironment>(RavenDb.GetFullId<DeploymentEnvironment>(id));
                if (config == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, string.Format("No environment with id [{0}] found!", id)));
                return Request.CreateResponse(HttpStatusCode.Found, config);
            }
            catch
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Something bad happend."));
            }
        }

        public HttpResponseMessage Put(ConDep.Server.Domain.Environment.Model.DeploymentEnvironment config)
        {
            Session.Store(config);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //private void Bogus()
        //{
        //    if (config == null)
        //    {
        //        config = new ConDepEnvConfig
        //        {
        //            EnvironmentName = "dev",
        //            Servers = new List<ServerConfig>()
        //                            {
        //                                new ServerConfig()
        //                                    {
        //                                        Name = "ec2-54-228-73-35.eu-west-1.compute.amazonaws.com"
        //                                    }
        //                            },
        //            DeploymentUser = new DeploymentUserConfig()
        //            {
        //                UserName = "Administrator",
        //                Password = "*S*RAvZ4D;t"
        //            }
        //        };
        //        session.Store(config);
        //    }
        //    else
        //    {
        //        config.EnvironmentName = "dev";
        //        config.Servers = new List<ServerConfig>()
        //                    {
        //                        new ServerConfig()
        //                            {
        //                                Name = "ec2-54-228-73-35.eu-west-1.compute.amazonaws.com"
        //                            }
        //                    };
        //        config.DeploymentUser = new DeploymentUserConfig()
        //        {
        //            UserName = "Administrator",
        //            Password = "*S*RAvZ4D;t"
        //        };
        //    }
        //    session.SaveChanges();
        //    //var env = session.Load<dynamic>("Environments");
        //    //var message = new HttpResponseMessage(HttpStatusCode.OK) { Content = env };
        //    //return message;
        //}
    }
}