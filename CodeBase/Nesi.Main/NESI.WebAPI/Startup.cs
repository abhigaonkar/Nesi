using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using NESI.WebAPI.Infrastructures.Authentication;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(NESI.WebAPI.Startup))]

namespace NESI.WebAPI
{
    public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
            var config = new HttpConfiguration();
            ConfigurationOAuth(app, config);

			WebApiConfig.Register(config);
		    MappingConfig.RegisterMaps();

			config.SuppressDefaultHostAuthentication();
			config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            SwaggerConfig.ManualRegister(config);
		}

		private static void ConfigurationOAuth(IAppBuilder app, System.Web.Http.HttpConfiguration config)
		{
			app.UseCors(CorsOptions.AllowAll);

			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
			var expiredTime = BLL.Common.Shared.Configuration.TokenExpiredTime;

			var oAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(expiredTime),
				Provider = new NesiAuthorizationServerProvider(),
				RefreshTokenProvider = new NesiRefreshTokenProvider(DateTime.Now.AddMinutes(expiredTime * 360))
			};

			// Token Generation
			app.UseOAuthAuthorizationServer(oAuthServerOptions);
			app.UseWebApi(config);
		}

	}
}