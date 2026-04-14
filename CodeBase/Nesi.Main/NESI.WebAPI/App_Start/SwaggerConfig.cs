using System.Web.Http;
using WebActivatorEx;
using NESI.WebAPI;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Linq;
using NESI.WebAPI.Infrastructures.Filters;
using System.Web;
using log4net;

namespace NESI.WebAPI
{
    /// <summary>
    /// Basic swagger configuration
    /// </summary>
    public class SwaggerConfig
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Manually invoke the Swagger registration instead of the default auto-wireup
        /// </summary>
        /// <param name="configuration"></param>
        public static void ManualRegister(HttpConfiguration configuration)
        {
            log.Info("Registering Swagger support...");
            //
            //  Display all controllers and actions.
            //
            configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Nesi API");
                    c.UseFullTypeNameInSchemaIds();
                    c.DocumentFilter<AuthTokenOperation>();
                    c.OAuth2("oauth2")
                        .Description("OAuth2")
                        .Flow("password")
                        .TokenUrl($"{VirtualPathUtility.ToAbsolute("~/")}token");
                    c.OperationFilter<AssignOAuth2SecurityRequirements>();
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("Nesi2 Web API");
                });
        }
    }

    /// <summary>
    /// Determine which operations require logging in based on any attributes
    /// </summary>
    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {
        /// <summary>
        /// Apply Security Measures.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // Determine if the operation or the controller has the Authorize attribute
            var authorizeAttributes = apiDescription.GetControllerAndActionAttributes<AuthorizeAttribute>();
            if (!authorizeAttributes.Any()) return;

            // Initialize the operation.security property
            if (operation.security == null)
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();

            // Add the appropriate security definition to the operation
            var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            {
                { "oauth2", Enumerable.Empty<string>() }
            };

            operation.security.Add(oAuthRequirements);
        }
    }

    /// <summary>
    /// Add a document filter that would allow us to obtain tokens in the swagger generated UI
    /// Allows us to put in creds and inspect the authorization result
    /// </summary>
    class AuthTokenOperation : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {
                    tags = new List<string> { "Authorization" },
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    parameters = new List<Parameter> {
                        new Parameter
                        {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData",
                            @default = "password"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "username",
                            required = true,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "password",
                            required = true,
                            @in = "formData",
                            format = "password"
                        }
                    }
                }
            });
        }
    }

}
