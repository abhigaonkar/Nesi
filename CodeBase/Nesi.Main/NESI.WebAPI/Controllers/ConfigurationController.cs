using System.Web.Http;

namespace NESI.WebAPI.Controllers
{
    [RoutePrefix("api/Configuration")]

    public class ConfigurationController : ApiController
    {
        public ApplicationConfigurationInformation Get()
        {
            var config = new ApplicationConfigurationInformation
            {
                ConfiguredEnvironment = BLL.Common.Shared.Configuration.Environment,
                IsDebugRedirect = BLL.Common.Shared.Configuration.DebugRedirect,
            };

            return config;
        }
    }

    /// <summary>
    /// Class that exposes information about the currently configured application
    /// </summary>
    /// <remarks>This class will be extended to provide additional information,
    /// and the controller that exposes this information will be automatically added to the project</remarks>
    public class ApplicationConfigurationInformation
    {
        public string ConfiguredEnvironment { get; set; }
        public bool IsDebugRedirect { get; set; }
    }

}
