using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using ConDep.Dsl.Remote.Node.Model;
using System.Linq;

namespace ConDep.Server.Api
{
    public static class ApiControllerExtensions
    {
        private const string BASE_REL = "http://www.con-dep.net/rels/server/";
        private const string SELF_REL = "self";
        private const string NEXT_REL = "next";
        private const string PREVIOUS_REL = "previous";

        public static string GetControllerUrl(this ApiController controller, params object[] args)
        {
            var urlBase = controller.Url.Link("Default", new { controller = GetControllerName(controller)});

            if (args.Any())
            {
                string urlPostFix = "";

                for (int i = 0; i < args.Length; i++)
                {
                    if (i != 0)
                        urlPostFix += "/{" + i + "}";
                    else
                        urlPostFix += "{" + i + "}";
                }
                var returnUrl = urlBase + "/" + urlPostFix;
                return string.Format(returnUrl, args);
            }
            return urlBase;
        }

        private static string GetControllerUrlOf<T>(ApiController controller , params object[] args) where T : ApiController
        {
            var urlBase = controller.Url.Link("Default", new { controller = GetControllerName<T>() });

            if (args.Any())
            {
                string urlPostFix = "";

                for (int i = 0; i < args.Length; i++)
                {
                    if(i != 0)
                        urlPostFix += "/{" + i + "}";
                    else
                        urlPostFix += "{" + i + "}";
                }

                var returnUrl = urlBase + "/" + urlPostFix;
                return string.Format(returnUrl, args);
            }
            return urlBase;
        }

        public static Link GetLink(this ApiController controller, HttpMethod httpMethod, params object[] hrefArgs)
        {
            return GetLink(controller, httpMethod, controller.GetRel(), hrefArgs);
        }

        public static Link GetLinkFor<T>(this ApiController controller, HttpMethod httpMethod, params object[] hrefArgs) where T : ApiController
        {
            return GetLink<T>(controller, httpMethod, GetRel<T>(), hrefArgs);
        }

        public static Link GetLinkPrevious(this ApiController controller, HttpMethod httpMethod, int page, params object[] hrefArgs)
        {
            return AddPageingToLink(page, GetLink(controller, httpMethod, PREVIOUS_REL, hrefArgs));
        }

        private static Link AddPageingToLink(int page, Link link)
        {
            link.Href = link.Href + "/?page=" + page;
            return link;
        }

        public static Link GetLinkNext(this ApiController controller, HttpMethod httpMethod, int page, params object[] hrefArgs)
        {
            return AddPageingToLink(page, GetLink(controller, httpMethod, NEXT_REL, hrefArgs));
        }

        private static Link GetLink(ApiController controller, HttpMethod httpMethod, string rel, params object[] hrefArgs)
        {
            return new Link
            {
                Href = controller.GetControllerUrl(hrefArgs),
                Method = httpMethod.ToString().ToUpper(),
                Rel = rel
            };
        }

        private static Link GetLink<T>(this ApiController controller, HttpMethod httpMethod, string rel, params object[] hrefArgs) where T : ApiController
        {
            return new Link
            {
                Href = GetControllerUrlOf<T>(controller, hrefArgs),
                Method = httpMethod.ToString().ToUpper(),
                Rel = rel
            };
        }

        public static Link GetLinkSelf(this ApiController controller, HttpMethod httpMethod, params object[] hrefArgs)
        {
            return GetLink(controller, httpMethod, SELF_REL, hrefArgs);
        }

        public static string GetRel(this ApiController controller)
        {
            return BASE_REL + GetControllerName(controller);
        }

        private static string GetRel<T>()
        {
            return BASE_REL + GetControllerName<T>();
        }

        private static string GetControllerName(ApiController controller)
        {
            return controller.GetType().Name.Replace("Controller", "").ToLower();
        }

        private static string GetControllerName<T>()
        {
            return typeof(T).Name.Replace("Controller", "").ToLower();
        }
    }
}