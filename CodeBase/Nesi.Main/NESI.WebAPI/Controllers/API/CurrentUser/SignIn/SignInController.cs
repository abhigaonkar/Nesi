using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using log4net;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.SignIn
{
	[RoutePrefix("api/SignIn")]
    public class SignInController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Route("RebootTime/{hostname}")]
	    public IHttpActionResult getRebootTime(string hostname)
		{
            logger.Debug($"Checking reboot time for '{hostname}'");
            try
            {
                return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem().GetRebootTime(hostname));
            }
            catch (Exception exception)
            {
                logger.Error($"Failed check", exception);
                throw;
            }
		}

	    public IHttpActionResult OkD(object obj)
	    {
		    return Ok(ControllerHelper.ConvertoJsonData(obj));
	    }
	}
}
