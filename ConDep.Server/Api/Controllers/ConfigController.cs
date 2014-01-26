using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Config;

namespace ConDep.Server.Api.Controllers
{
    public class ConfigController : RavenDbController
    {
        private const string DOC_ID_PREFIX = "environments";
        private const string DOC_ID_TEMPLATE = DOC_ID_PREFIX + "/{0}";

        public ConDepEnvConfig[] Get()
        {
            try
            {
                return Session.Advanced.LoadStartingWith<ConDepEnvConfig>(DOC_ID_PREFIX) ?? new ConDepEnvConfig[0];
            }
            catch
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Something bad happend."));
            }
        }

        public ConDepEnvConfig Get(string id)
        {
            try
            {
                var config = Session.Load<ConDepEnvConfig>(string.Format(DOC_ID_TEMPLATE, id));
                if (config == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, string.Format("No environment with id [{0}] found!", id)));
                return config;
            }
            catch
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Something bad happend."));
            }
        }

        public HttpResponseMessage Put(ConDepEnvConfig config)
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