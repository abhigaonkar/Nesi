using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Tracing;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using TraceLevel = System.Diagnostics.TraceLevel;

namespace Nesi.Web
{
    public static class WebApiConfig
    {
        public static string UrlPrefix => "api";
        public static string UrlPrefixRelative => "~/api";

       
        public static void Register(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: $"{UrlPrefix}/{{controller}}/{{id}}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            var cors = new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true };
            config.EnableCors(cors);
#if DEBUG
            config.EnableSystemDiagnosticsTracing();
            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = System.Web.Http.Tracing.TraceLevel.Debug;
#endif
        }
    }

    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }

    public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }

}
