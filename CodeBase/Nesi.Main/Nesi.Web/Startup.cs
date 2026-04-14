using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens;
using System.Web.Http;
using System.Web.Routing;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(Nesi.Web.Startup))]

namespace Nesi.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //
            //  Add web api support to the project
            //
            GlobalConfiguration.Configure(WebApiConfig.Register);
            WebApiConfig.Register(RouteTable.Routes);
            MappingConfig.RegisterMaps();
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

    }
}
