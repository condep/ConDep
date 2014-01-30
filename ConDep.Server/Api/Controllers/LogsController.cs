using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Config;
using ConDep.Dsl.Remote.Node.Model;
using ConDep.Server.Domain.Deployment.Model;
using Raven.Client;

namespace ConDep.Server.Api.Controllers
{
    public class LogsController : ApiController
    {
        public IEnumerable<Link> Get()
        {
            return GetEnvironmentLinks();
        }

        public dynamic Get(string env, int pageSize = 10, int page = 0)
        {
            return GetLogLinksForEnvironment(env, pageSize, page);
        }

        private dynamic GetLogLinksForEnvironment(string env, int pageSize, int page)
        {
            var logs = new List<Link>();
            int totalPages;
            int totalResults;

            using(var session = RavenDb.DocumentStore.OpenSession())
            {
                RavenQueryStatistics stats;
                var items = session.Query<Deployment, ExecutionStatus_ByEnvironment>()
                                   .Statistics(out stats)
                                   .Skip(page * pageSize)
                                   .Take(pageSize)
                                   .Where(x => x.Environment.Equals(env, StringComparison.InvariantCultureIgnoreCase))
                                   .OrderByDescending(x => x.StartedUtc).ToList();

                totalResults = stats.TotalResults;
                var remainder = totalResults%pageSize > 0 ? 1 : 0;
                totalPages = totalResults/pageSize + remainder;

                logs.AddRange(items.Select(item => this.GetLinkFor<LogController>(HttpMethod.Get, item.Id)));
            }

            return new
                {
                    Environment = env,
                    CurrentPage = page,
                    LastPage = totalPages - 1,
                    ItemsOnPage = logs.Count,
                    ItemsInTotal = totalResults,
                    Next = page + 1 <= totalPages - 1
                               ? this.GetLinkNext(HttpMethod.Get, page + 1, env)
                               : null,
                    Previous = page - 1 >= 0
                                   ? this.GetLinkPrevious(HttpMethod.Get, page - 1, env)
                                   : null,
                    Logs = logs
                };
        }

        private IEnumerable<Link> GetEnvironmentLinks()
        {
            using (var session = RavenDb.DocumentStore.OpenSession())
            {
                var environments = session.Query<ConDepEnvConfig>();
                foreach (var env in environments)
                {
                    yield return this.GetLink(HttpMethod.Get, env.EnvironmentName);
                }
            }
        }

    }
}