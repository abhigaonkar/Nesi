using System;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using core;
using FluentValidation.WebApi;
using Newtonsoft.Json.Serialization;
using NESI.Data.Entities;
using NESI.WebAPI.Infrastructures.Filters;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static core.XpoUtility;


namespace NESI.WebAPI
{
    public static class WebApiConfig
    {
        private static readonly ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
	        // enable CORS for all hosts, headers and methods
	        var cors = new EnableCorsAttribute("*", "*", "*") {SupportsCredentials = true};
	        config.Filters.Add(new ValidateModelAttribute());
	        config.Filters.Add(new LogPageAttribute());
#if !DEBUG
			config.Filters.Add(new NesiErrorHandlerFilterAttribute());
#endif
			//  config.MessageHandlers.Add(new ResponseWrappingHandler());

			config.EnableCors(cors);
			config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

	        FluentValidationModelValidatorProvider.Configure(config);

            /*
             * Removed due to high startup impact
             */
			//RegisterODataServices(config);

	        IsoDateTimeConverter converter = new IsoDateTimeConverter
            {
                //DateTimeStyles = DateTimeStyles.AdjustToUniversal,
                DateTimeFormat = "yyyy-MM-dd"
            };

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(converter);

            StoreCoreXpoAssemblies(BLL.Common.Shared.Configuration.XpoConnection);

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
	        jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}

        /// <summary>
        /// Register odata endpoints
        /// </summary>
        /// <param name="config"></param>
        private static void RegisterODataServices(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<business_unit>("business_unit");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
	}
}
